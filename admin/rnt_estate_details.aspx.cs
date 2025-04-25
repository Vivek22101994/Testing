using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.admin
{
    public partial class rnt_estate_details : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        private magaRental_DataContext DC_RENTAL;
        protected string listPage = "rnt_estate_list.aspx";
        private RNT_TB_ESTATE _currTBL;
        public int Id_currTBL
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
            }
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                lnk_saveOnly.Visible = UserAuthentication.CurrentUserID == 2;
                lnk_salva.Visible = lnk_annulla.Visible = UserAuthentication.hasPermission("rnt_estate", "can_edit");
                _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == Request.QueryString["id"].ToInt32());
                if (_currTBL == null)
                    Response.Redirect(listPage);
                Id_currTBL = Request.QueryString["id"].ToInt32();
                UC_rnt_estate_navlinks1.IdEstate = Id_currTBL;
                Bind_drp_residence();
                Bind_drp_city();
                Bind_drp_zone();
                drp_owner.DataBind();
                drp_importance_vote.bind_Numbers(1, 10, 1, 0);
                drp_lpb_afterdays.bind_Numbers(90, 360, 30, 0);
                drp_lpb_nights_min.bind_Numbers(5, 15, 1, 0);

                List<RntHaPropertyTypeLK> _currTBLproperty = DC_RENTAL.RntHaPropertyTypeLK.ToList();
                drp_Property.DataSource = _currTBLproperty;
                drp_Property.DataTextField = "value";
                drp_Property.DataValueField = "value";
                drp_Property.DataBind();
                //drp_Property.Items.Insert(0, new ListItem("Select Property Type", "0"));


                FillControls();
            }
            else
            {
            }
        }
        protected void btn_page_update_Click(object sender, EventArgs e)
        {
            if (HF_updater_args.Value == "drp_owner")
            {
                drp_owner.DataBind();
                drp_owner.setSelectedValue(HF_updater_newId.Value);
            }
        }
        private void Bind_drp_residence()
        {
            List<RNT_TB_RESIDENCE> list = DC_RENTAL.RNT_TB_RESIDENCEs.Where(x => x.is_deleted != 1 && x.is_active == 1).ToList();
            drp_residence.Items.Clear();
            foreach (RNT_TB_RESIDENCE t in list)
            {
                drp_residence.Items.Add(new ListItem(t.code, "" + t.id));
            }
            drp_residence.Items.Insert(0, new ListItem("- nessuna -", "0"));
        }
        private void Bind_drp_city()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_city.Items.Clear();
            foreach (LOC_VIEW_CITY t in list)
            {
                drp_city.Items.Add(new ListItem(t.title, "" + t.id));
            }
            drp_city.Items.Insert(0, new ListItem("-seleziona-", "0"));
        }
        private void Bind_drp_zone()
        {
            List<LOC_VIEW_ZONE> list = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == 1 && x.pid_city == drp_city.getSelectedValueInt(0)).ToList();
            drp_zone.Items.Clear();
            foreach (LOC_VIEW_ZONE t in list)
            {
                drp_zone.Items.Add(new ListItem(t.title, "" + t.id));
            }
            drp_zone.Items.Insert(0, new ListItem("-seleziona-", "0"));
        }
        protected void drp_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_drp_zone();
        }
        protected void drp_owner_DataBound(object sender, EventArgs e)
        {
            drp_owner.Items.Insert(0, new ListItem("- seleziona -", ""));
        }
        private void FillControls()
        {
            _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(item => item.id == Id_currTBL);
            if (_currTBL == null)
            {
                _currTBL = new RNT_TB_ESTATE();
                // default values
                _currTBL.num_persons_adult = 2;
                _currTBL.num_persons_child = 2;
            }


            //CURRENT_LANG = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == _currTBL.id).ToList();
            //HF_lang.Value = "1";

            drp_owner.setSelectedValue(_currTBL.pid_owner.ToString());
            txt_ext_ownerdaysinyear.Text = _currTBL.ext_ownerdaysinyear.ToString();

            drp_category.setSelectedValue(_currTBL.category);
            txt_code.Text = _currTBL.code ?? "";
            txt_address.Text = _currTBL.loc_address;
            txt_loc_inner_bell.Text = _currTBL.loc_inner_bell;
            txt_phone_1.Text = _currTBL.loc_phone_1;
            txt_phone_2.Text = _currTBL.loc_phone_2;
            txt_zip_code.Text = _currTBL.loc_zip_code;
            txt_inner_notes.Text = _currTBL.inner_notes ?? "";

            drp_residence.setSelectedValue(_currTBL.pid_residence.ToString());
            drp_city.setSelectedValue(_currTBL.pid_city.ToString());
            Bind_drp_zone();
            drp_zone.setSelectedValue(_currTBL.pid_zone.ToString());


            txt_num_bed_single.Text = _currTBL.num_bed_single.objToInt32().ToString();
            txt_num_bed_double.Text = _currTBL.num_bed_double.objToInt32().ToString();
            txt_num_bed_double_divisible.Text = _currTBL.num_bed_double_divisible.objToInt32().ToString();
            txt_num_bed_double_2level.Text = _currTBL.num_bed_double_2level.objToInt32().ToString();
            txt_num_sofa_single.Text = _currTBL.num_sofa_single.objToInt32().ToString();
            txt_num_sofa_double.Text = _currTBL.num_sofa_double.objToInt32().ToString();
            txt_num_beds.Text = _currTBL.num_persons_adult.objToInt32().ToString();
            txt_num_beds_optional.Text = _currTBL.num_persons_optional.objToInt32().ToString();
            txt_num_persons_child.Text = _currTBL.num_persons_child.objToInt32().ToString();
            txt_num_persons_min.Text = _currTBL.num_persons_min.objToInt32().ToString();

            txt_pr_deposit.Text = _currTBL.pr_deposit.objToInt32().ToString();
            drp_pr_depositWithCard.setSelectedValue(_currTBL.pr_depositWithCard.ToString());
            txt_pr_percentage.Text = _currTBL.pr_percentage.objToInt32().ToString();
            txt_nights_minVHSeason.Text = _currTBL.nights_minVHSeason.objToInt32().ToString();
            txt_nights_min.Text = _currTBL.nights_min.objToInt32().ToString();
            txt_nights_max.Text = _currTBL.nights_max.objToInt32().ToString();

            txt_floor.Text = _currTBL.on_floor.objToInt32().ToString();
            txt_mq_inner.Text = _currTBL.mq_inner.objToInt32().ToString();
            txt_sqFeet.Text = _currTBL.sqFeet.objToInt32() + "";
            txt_mq_outer.Text = _currTBL.mq_outer.objToInt32().ToString();
            txt_mq_terrace.Text = _currTBL.mq_terrace.objToInt32().ToString();
            txt_num_rooms_bed.Text = _currTBL.num_rooms_bed.objToInt32().ToString();
            txt_num_kitchen.Text = _currTBL.num_kitchen.objToInt32().ToString();
            txt_num_room_living.Text = _currTBL.num_rooms_living.objToInt32().ToString();
            txt_num_terraces.Text = _currTBL.num_terraces.objToInt32().ToString();
            txt_bath_rooms.Text = _currTBL.num_rooms_bath.objToInt32().ToString();

            txt_eco_pr_1.Text = _currTBL.eco_pr_1.objToInt32().ToString();
            txt_eco_pr_2.Text = _currTBL.eco_pr_2.objToInt32().ToString();
            txt_eco_time_preview.Text = _currTBL.eco_time_preview;
            txt_eco_notes.Text = _currTBL.eco_notes;


            drp_importance_vote.setSelectedValue(_currTBL.importance_vote.ToString());
            chk_is_active.Checked = _currTBL.is_active == 1;
            chk_is_online_booking.Checked = _currTBL.is_online_booking == 1;
            chk_is_google_maps.Checked = _currTBL.is_google_maps == 1;
            chk_is_exclusive.Checked = _currTBL.is_exclusive == 1;
            chk_pr_has_overnight_tax.Checked = _currTBL.pr_has_overnight_tax == 1;
            chk_isCedolareSecca.Checked = _currTBL.isCedolareSecca == 1;
            chk_isContratto.Checked = _currTBL.isContratto == 1;

            // longTermRent
            drp_longTermRent.setSelectedValue(_currTBL.longTermRent.ToString());
            txt_longTermPrMonthly.Value = _currTBL.longTermPrMonthly.objToDouble();

            // lpb LongPeriodBooking
            drp_lpb_is.setSelectedValue(_currTBL.lpb_is.ToString());
            drp_lpb_afterdays.setSelectedValue(_currTBL.lpb_afterdays.ToString());
            drp_lpb_nights_min.setSelectedValue(_currTBL.lpb_nights_min.ToString());
            drp_lpb_onlyhighseason.setSelectedValue(_currTBL.lpb_onlyhighseason.ToString());

            // eco
            drp_is_ecopulizie.setSelectedValue(_currTBL.is_ecopulizie.ToString());
            txt_eco_ext_name_full.Text = _currTBL.eco_ext_name_full;
            txt_eco_ext_email.Text = _currTBL.eco_ext_email;
            txt_eco_ext_phone.Text = _currTBL.eco_ext_phone;
            txt_eco_ext_price.Text = _currTBL.eco_ext_price.objToInt32().ToString();
            drp_eco_ext_clientPay.setSelectedValue(_currTBL.eco_ext_clientPay.ToString());
            txt_eco_ext_payInDays.Text = _currTBL.eco_ext_payInDays.objToInt32().ToString();

            // srs
            drp_is_srs.setSelectedValue(_currTBL.is_srs.ToString());
            txt_srs_ext_name_full.Text = _currTBL.srs_ext_name_full;
            txt_srs_ext_email.Text = _currTBL.srs_ext_email;
            txt_srs_ext_phone.Text = _currTBL.srs_ext_phone;
            txt_srs_ext_phone_2.Text = _currTBL.srs_ext_phone_2;
            txt_srs_ext_phone_3.Text = _currTBL.srs_ext_phone_3;
            //txt_srs_ext_phone_4.Text = _currTBL.srs_ext_phone_4;
            txt_srs_ext_price.Text = _currTBL.srs_ext_price.objToInt32().ToString();
            drp_srs_ext_clientPay.setSelectedValue(_currTBL.srs_ext_clientPay.ToString());
            txt_srs_ext_meetingPoint.Text = _currTBL.srs_ext_meetingPoint;
            chk_children_allowed.Checked = _currTBL.is_chidren_allowed == 1 ? true : false;

            // temp
            //t_piano.Text = _currTBL.t_piano;
            //t_terrazzo.Text = _currTBL.t_terrazzo;
            //t_propCognome.Text = _currTBL.t_propCognome;
            //t_propNome.Text = _currTBL.t_propNome;
            //t_propEmail.Text = _currTBL.t_propEmail;
            //t_propContatti.Text = _currTBL.t_propContatti;
            //t_propCFPI.Text = _currTBL.t_propCFPI;
            //t_propIndirizzo.Text = _currTBL.t_propIndirizzo;
            //t_mezzi.Text = _currTBL.t_mezzi;
            //t_modPagamento.Text = _currTBL.t_modPagamento;
            //t_propWeb.Text = _currTBL.t_propWeb;
            //t_metriQ.Text = _currTBL.t_metriQ;
            //t_extra.Text = _currTBL.t_extra;
            //t_link.Text = _currTBL.t_link;

            //property type dropdown list


            if (_currTBL.haPropertyType != null)
                drp_Property.setSelectedValue(_currTBL.haPropertyType.ToString());

            //advertiser id
            //txt_advertiserID.Text = _currTBL.haAdvertiserId;
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            {
                var ownerTbl = dcChnl.dbRntChnlHomeAwayOwnerTBLs.FirstOrDefault();
                if (ownerTbl != null)
                {
                    drpHomeAwayAdvertiser.Items.Add("");
                    if (!string.IsNullOrEmpty(ownerTbl.advertiserAssignedId))
                        drpHomeAwayAdvertiser.Items.Add(ownerTbl.advertiserAssignedId);
                    if (!string.IsNullOrEmpty(ownerTbl.ppb_advertiserAssignedId) && (string.IsNullOrEmpty(ownerTbl.advertiserAssignedId) || ownerTbl.advertiserAssignedId != ownerTbl.ppb_advertiserAssignedId))
                        drpHomeAwayAdvertiser.Items.Add(ownerTbl.ppb_advertiserAssignedId);
                    drpHomeAwayAdvertiser.setSelectedValue(_currTBL.haAdvertiserId);
                }
            }

            //listing id
            txt_listingId.Text = string.IsNullOrEmpty(_currTBL.haListingId) ? _currTBL.id + "" : _currTBL.haListingId;
            //isHomeaway
            chk_homeAway.Checked = _currTBL.is_HomeAway == 1;

            //Fill_lang(HF_lang.Value.ToInt32());
            //LV_langs.DataBind();
            RegisterScripts();
            //DisableControls();
        }
        private void FillDataFromControls()
        {
            RNT_TB_ESTATE tbBefore = null;
            DC_RENTAL.CommandTimeout = 0;
            _currTBL = new RNT_TB_ESTATE();
            if (Id_currTBL != 0)
            {
                _currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(item => item.id == Id_currTBL);
                tbBefore = _currTBL.Clone();
            }
            else
            {
                DC_RENTAL.RNT_TB_ESTATE.InsertOnSubmit(_currTBL);
            }
            bool is_srs = _currTBL.is_srs == 1;
            bool is_ecopulizie = _currTBL.is_ecopulizie == 1;
            _currTBL.category = drp_category.SelectedValue;

            _currTBL.num_bed_single = txt_num_bed_single.Text.ToInt32();
            _currTBL.num_bed_double = txt_num_bed_double.Text.ToInt32();
            _currTBL.num_bed_double_divisible = txt_num_bed_double_divisible.Text.ToInt32();
            _currTBL.num_bed_double_2level = txt_num_bed_double_2level.Text.ToInt32();
            _currTBL.num_sofa_single = txt_num_sofa_single.Text.ToInt32();
            _currTBL.num_sofa_double = txt_num_sofa_double.Text.ToInt32();
            _currTBL.num_persons_adult = txt_num_beds.Text.ToInt32();
            _currTBL.num_persons_optional = txt_num_beds_optional.Text.ToInt32();
            _currTBL.num_persons_child = txt_num_persons_child.Text.ToInt32();
            _currTBL.num_persons_max = _currTBL.num_persons_adult + _currTBL.num_persons_optional;
            _currTBL.num_persons_min = txt_num_persons_min.Text.ToInt32();

            _currTBL.num_rooms_bed = txt_num_rooms_bed.Text.ToInt32();
            _currTBL.num_rooms_bath = txt_bath_rooms.Text.ToInt32();
            _currTBL.num_kitchen = txt_num_kitchen.Text.objToInt32();
            _currTBL.num_rooms_living = txt_num_room_living.objToInt32();
            _currTBL.pr_deposit = txt_pr_deposit.Text.ToInt32();
            _currTBL.pr_depositWithCard = drp_pr_depositWithCard.getSelectedValueInt(0);
            _currTBL.pr_percentage = txt_pr_percentage.Text.ToInt32();

            _currTBL.code = txt_code.Text.Trim();
            _currTBL.inner_notes = txt_inner_notes.Text.Trim();
            _currTBL.on_floor = txt_floor.Text.ToInt32();
            _currTBL.mq_inner = txt_mq_inner.Text.ToInt32();
            _currTBL.sqFeet = txt_sqFeet.Text.ToInt32();
            _currTBL.mq_outer = txt_mq_outer.Text.ToInt32();
            _currTBL.mq_terrace = txt_mq_terrace.Text.ToInt32();
            _currTBL.num_terraces = txt_num_terraces.Text.ToInt32();
            _currTBL.nights_minVHSeason = txt_nights_minVHSeason.Text.ToInt32();
            _currTBL.nights_min = txt_nights_min.Text.ToInt32();
            _currTBL.nights_max = txt_nights_max.Text.ToInt32();

            _currTBL.eco_pr_1 = txt_eco_pr_1.Text.ToInt32();
            _currTBL.eco_pr_2 = txt_eco_pr_2.Text.ToInt32();
            _currTBL.eco_time_preview = txt_eco_time_preview.Text;
            _currTBL.eco_notes = txt_eco_notes.Text;


            _currTBL.loc_inner_bell = txt_loc_inner_bell.Text;
            _currTBL.loc_address = txt_address.Text;
            _currTBL.loc_zip_code = txt_zip_code.Text;
            _currTBL.loc_phone_1 = txt_phone_1.Text;
            _currTBL.loc_phone_2 = txt_phone_2.Text;

            _currTBL.pid_agent = 0;
            _currTBL.pid_residence = drp_residence.getSelectedValueInt(0);
            _currTBL.pid_city = drp_city.getSelectedValueInt(0);
            _currTBL.pid_zone = drp_zone.getSelectedValueInt(0);
            _currTBL.pid_owner = drp_owner.getSelectedValueInt(0);
            _currTBL.ext_ownerdaysinyear = txt_ext_ownerdaysinyear.Text.ToInt32();
            _currTBL.pid_category = 0;
            _currTBL.pid_type = 0;


            _currTBL.importance_vote = drp_importance_vote.getSelectedValueInt(1);
            _currTBL.is_active = chk_is_active.Checked ? 1 : 0;
            _currTBL.is_google_maps = chk_is_google_maps.Checked ? 1 : 0;
            _currTBL.is_online_booking = chk_is_online_booking.Checked ? 1 : 0;
            _currTBL.is_exclusive = chk_is_exclusive.Checked ? 1 : 0;
            _currTBL.pr_has_overnight_tax = chk_pr_has_overnight_tax.Checked ? 1 : 0;
            _currTBL.isCedolareSecca = chk_isCedolareSecca.Checked ? 1 : 0;
            _currTBL.isContratto = chk_isContratto.Checked ? 1 : 0;


            // longTermRent
            _currTBL.longTermRent = drp_longTermRent.getSelectedValueInt(1);
            _currTBL.longTermPrMonthly = txt_longTermPrMonthly.Value.objToDecimal();

            // lpb LongPeriodBooking
            _currTBL.lpb_is = drp_lpb_is.getSelectedValueInt(1);
            _currTBL.lpb_afterdays = drp_lpb_afterdays.getSelectedValueInt(1);
            _currTBL.lpb_nights_min = drp_lpb_nights_min.getSelectedValueInt(1);
            _currTBL.lpb_onlyhighseason = drp_lpb_onlyhighseason.getSelectedValueInt(1);

            // eco
            _currTBL.is_ecopulizie = drp_is_ecopulizie.getSelectedValueInt(1);
            _currTBL.eco_ext_name_full = txt_eco_ext_name_full.Text;
            _currTBL.eco_ext_email = txt_eco_ext_email.Text;
            _currTBL.eco_ext_phone = txt_eco_ext_phone.Text;
            _currTBL.eco_ext_price = txt_eco_ext_price.Text.ToInt32();
            _currTBL.eco_ext_clientPay = drp_eco_ext_clientPay.getSelectedValueInt(1);
            _currTBL.eco_ext_payInDays = txt_eco_ext_payInDays.Text.ToInt32();

            // srs
            _currTBL.is_srs = drp_is_srs.getSelectedValueInt(1);
            _currTBL.srs_ext_name_full = txt_srs_ext_name_full.Text;
            _currTBL.srs_ext_email = txt_srs_ext_email.Text;
            _currTBL.srs_ext_phone = txt_srs_ext_phone.Text;
            _currTBL.srs_ext_phone_2 = txt_srs_ext_phone_2.Text;
            _currTBL.srs_ext_phone_3 = txt_srs_ext_phone_3.Text;
            //_currTBL.srs_ext_phone_4 = txt_srs_ext_phone_4.Text;
            _currTBL.srs_ext_price = txt_srs_ext_price.Text.ToInt32();
            _currTBL.srs_ext_clientPay = drp_srs_ext_clientPay.getSelectedValueInt(1);
            _currTBL.srs_ext_meetingPoint = txt_srs_ext_meetingPoint.Text;
            _currTBL.is_chidren_allowed = chk_children_allowed.Checked == true ? 1 : 0;

            //property type
            _currTBL.haPropertyType = drp_Property.SelectedValue;

            //is homeaway           
            _currTBL.is_HomeAway = chk_homeAway.Checked ? 1 : 0;

            //advertiser id
            //_currTBL.haAdvertiserId = txt_advertiserID.Text;
            _currTBL.haAdvertiserId = drpHomeAwayAdvertiser.SelectedValue;

            //listing id
            _currTBL.haListingId = txt_listingId.Text;

            if (!Request.Url.AbsoluteUri.Contains("http://localhost"))
            {
                string _coords = "";
                string _address = _currTBL.loc_address;
                if (is_srs && _currTBL.is_srs != 1)
                {
                    _coords = _currTBL.google_maps ?? "";
                    _coords = _coords.Replace(",", ".").Replace("|", ",");
                    Srs_WS.Location_Insert_Update(_currTBL.code, CurrentSource.locZone_title(_currTBL.pid_zone.objToInt32(), 1, "---"), _address, _currTBL.id, 0, _currTBL.num_bed_single.objToInt32(), _currTBL.num_bed_double.objToInt32(), _currTBL.num_rooms_bath.objToInt32(), 0, _coords);
                }
                if (_currTBL.is_srs == 1)
                {
                    _coords = _currTBL.google_maps ?? "";
                    _coords = _coords.Replace(",", ".").Replace("|", ",");
                    Srs_WS.Location_Insert_Update(_currTBL.code, CurrentSource.locZone_title(_currTBL.pid_zone.objToInt32(), 1, "---"), _address, _currTBL.id, 1, _currTBL.num_bed_single.objToInt32(), _currTBL.num_bed_double.objToInt32(), _currTBL.num_rooms_bath.objToInt32(), 0, _coords);
                    Srs_WS.EstateReservations_UpdateAll(_currTBL.id);
                }
                if (is_ecopulizie && _currTBL.is_ecopulizie != 1)
                {
                    Eco_WS.Location_Insert_Update(_currTBL);
                }
                if (_currTBL.is_ecopulizie == 1)
                {
                    Eco_WS.Location_Insert_Update(_currTBL);
                    Eco_WS.EstateReservations_UpdateAll(_currTBL.id);
                }
            }

            DC_RENTAL.CommandTimeout = 0;
            DC_RENTAL.SubmitChanges();
            //Save_RL_langs();
            if (tbBefore != null) rntUtils.estate_addLog(tbBefore, _currTBL, UserAuthentication.CurrentUserID, UserAuthentication.CurrentUserName);
            rntUtils.rntEstate_createPagePath(_currTBL.id);
            AdminUtilities.FillRewriteTool();
            AppSettings._refreshCache_RNT_ESTATEs();
            AppSettings.RELOAD_SESSION();
            //if (_currTBL.bcomEnabled == 1)
            //BcomUpdate.BcomUpdate_start(_currTBL.id, "rates");
            rntUtilsChnlAll.UpdateRates(_currTBL.id);

        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            //DisableControls();
            Response.Redirect(listPage);
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            RegisterScripts();
        }
        private void RegisterScripts()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                "tinyEditor",
                                                "setTinyEditor(false);", true);
        }
        //protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    Label lbl_id = (Label)e.Item.FindControl("lbl_id");
        //    LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
        //    lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
        //    List<RNT_LN_ESTATE> _rList = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_lang == lbl_id.Text.ToInt32() && x.pid_estate == Id_currTBL).ToList();
        //    if (_rList.Count == 0) lnk.CssClass += " important1";
        //    RNT_LN_ESTATE _lang = _rList.FirstOrDefault(x => x.title == null || x.title.Trim() == "" || x.meta_title == null || x.meta_title.Trim() == "" || x.meta_description == null || x.meta_description.Trim() == "" || x.description == null || x.description.Trim() == "");
        //    if (_lang != null) lnk.CssClass += " important2";
        //}

        //protected void LV_langs_ItemCommand(object sender, ListViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "change_lang")
        //    {
        //        Label lbl_id = (Label)e.Item.FindControl("lbl_id");
        //        Save_lang();
        //        HF_lang.Value = lbl_id.Text;
        //        Fill_lang(HF_lang.Value.ToInt32());
        //        LV_langs.DataBind();
        //        RegisterScripts();
        //    }
        //}

        //protected void Save_lang()
        //{
        //    var curr_rl_langs = CURRENT_LANG;
        //    var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == HF_lang.Value.ToInt32());
        //    if (rlLang == null)
        //    {
        //        rlLang = new RNT_LN_ESTATE();
        //        rlLang.pid_estate = Id_currTBL;
        //        rlLang.pid_lang = int.Parse(HF_lang.Value);
        //        CURRENT_LANG.Add(rlLang);

        //    }
        //    rlLang.description = re_description.Content;
        //    rlLang.title = txt_title.Text;
        //    rlLang.sub_title = txt_sub_title.Text;
        //    rlLang.summary = txt_summary.Text;
        //    rlLang.meta_description = txt_meta_description.Text;
        //    rlLang.meta_keywords = txt_meta_keywords.Text;
        //    rlLang.meta_title = txt_meta_title.Text;
        //    rlLang.mobileDescription = txt_mobileDescription.Text;
        //    rlLang.haHeadLine = txt_headLine.Text;
        //    rlLang.haDescription = re_haDescription.Content;
        //    rlLang.haOtherActivities = re_haOtherActivities.Content;
        //    rlLang.haRateNote = re_rateNotes.Content;

        //    //rlLang.haR


        //    CURRENT_LANG = curr_rl_langs;
        //}

        //protected void Fill_lang(int pid_lang)
        //{
        //    var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == pid_lang);
        //    if (rlLang == null)
        //    {
        //        rlLang = new RNT_LN_ESTATE();
        //    }
        //    txt_title.Text = rlLang.title;
        //    txt_sub_title.Text = rlLang.sub_title;
        //    re_description.Content = rlLang.description;
        //    txt_summary.Text = rlLang.summary;
        //    txt_meta_description.Text = rlLang.meta_description;
        //    txt_meta_keywords.Text = rlLang.meta_keywords;
        //    txt_meta_title.Text = rlLang.meta_title;
        //    txt_mobileDescription.Text = rlLang.mobileDescription;
        //    txt_headLine.Text = rlLang.haHeadLine;
        //    re_haDescription.Content = rlLang.haDescription;
        //    re_haOtherActivities.Content = rlLang.haOtherActivities;
        //    re_rateNotes.Content = rlLang.haRateNote;

        //}

        //protected void Save_RL_langs()
        //{
        //    Save_lang();
        //    var curr_rl_langs = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == _currTBL.id).ToList();
        //    foreach (var rl in CURRENT_LANG)
        //    {
        //        if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
        //        {
        //            rl.pid_estate = _currTBL.id;
        //            DC_RENTAL.RNT_LN_ESTATE.InsertOnSubmit(rl);
        //        }
        //        else
        //        {
        //            var curr_rl = curr_rl_langs.Single(x => x.pid_lang == rl.pid_lang);
        //            curr_rl.description = rl.description;
        //            curr_rl.meta_description = rl.meta_description;
        //            curr_rl.meta_keywords = rl.meta_keywords;
        //            curr_rl.meta_title = rl.meta_title;
        //            curr_rl.page_path = rl.page_path;
        //            curr_rl.title = rl.title;
        //            curr_rl.sub_title = rl.sub_title;
        //            curr_rl.summary = rl.summary;
        //            curr_rl.mobileDescription = rl.mobileDescription;
        //            curr_rl.haHeadLine = rl.haHeadLine;
        //            curr_rl.haDescription = rl.haDescription;
        //            curr_rl.haOtherActivities = rl.haOtherActivities;
        //            curr_rl.haRateNote = rl.haRateNote;


        //        }
        //        // --- genarate PagePath & Rewrite Tool
        //    }
        //    DC_RENTAL.SubmitChanges();
        //    curr_rl_langs = DC_RENTAL.RNT_LN_ESTATE.Where(x => x.pid_estate == _currTBL.id).ToList();
        //    foreach (CONT_TBL_LANG _lang in maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.Where(x => x.is_active == 1))
        //    {
        //        if (!curr_rl_langs.Exists(x => x.pid_lang == _lang.id))
        //        {
        //            RNT_LN_ESTATE _newRL = new RNT_LN_ESTATE();
        //            _newRL.pid_lang = _lang.id;
        //            _newRL.pid_estate = _currTBL.id;
        //            //_newRL.title = txt_code.Text;
        //            DC_RENTAL.RNT_LN_ESTATE.InsertOnSubmit(_newRL);
        //        }
        //    }
        //    DC_RENTAL.SubmitChanges();
        //}
        //protected void lnk_copyLang_Click(object sender, EventArgs e)
        //{
        //    HF_copyLang.Value = HF_lang.Value;
        //    lnk_pasteLang.Visible = true;
        //    ScriptManager.RegisterStartupScript(this, this.GetType(),
        //                                       "tinyEditor",
        //                                       "setTinyEditor(false);", true);
        //}
        //protected void lnk_pasteLang_Click(object sender, EventArgs e)
        //{
        //    Fill_lang(HF_copyLang.Value.ToInt32());
        //    ScriptManager.RegisterStartupScript(this, this.GetType(),
        //                                        "tinyEditor",
        //                                        "setTinyEditor(false);", true);
        //}
    }
}
