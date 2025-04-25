using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class EstateChnlHoliday_unit : adminBasePage
    {
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].objToInt32();
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                fillData();
            }
        }
        protected void fillData()
        {
            var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currEstate == null)
            {
                //ErrorLog.addLog("", "ChnlHolidayUpdate.UpdateRates_process", "idEstate:" + IdEstate + " not found or not active");
                return;
            }
            using (DCmodRental dc = new DCmodRental())
            {
                var currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null || currTbl.homeId.ToInt64() <= 0)
                {
                    Response.Redirect("EstateChnlHoliday_main.aspx?id=" + IdEstate);
                    return;
                }

                drp_num_bedrooms.Enabled = !currTbl.ActiveState;

                drp_bathrooms.bind_Numbers(0, 16, 1, 1);
                drp_en_suite.bind_Numbers(0, 25, 1, 1);
                drp_shower_rooms.bind_Numbers(0, 16, 1, 1);
                drp_single_beds.bind_Numbers(0, 50, 1, 1);
                drp_double_beds.bind_Numbers(0, 25, 1, 1);
                drp_sofa_beds.bind_Numbers(0, 26, 1, 1);
                drp_cots.bind_Numbers(0, 16, 1, 1);
                drp_longue_seats.bind_Numbers(0, 50, 1, 1);
                drp_dining_seats.bind_Numbers(0, 50, 1, 1);
                drp_num_bedrooms.bind_Numbers(1, 50, 1, 1);
                drp_max_guest.bind_Numbers(currEstate.num_persons_min.objToInt32(), currEstate.num_persons_max.objToInt32(), 1, 1);
                drp_max_guest.Items.Insert(0, new ListItem("Select", "0"));
                drp_num_bedrooms.Items.Insert(0, new ListItem("Select", ""));

                txt_home_id.Text = currTbl.homeId;
                txt_SiteId.Text = currTbl.SiteId + "";
                txt_ActiveState.Text = currTbl.ActiveState?"Yes":"No";
                txt_name.Text = currTbl.homeName;
                txt_summary.Text = currTbl.homeSummary;
                txt_unit_description.Text = currTbl.homeDescription;

                //features
                drp_num_bedrooms.setSelectedValue(currTbl.num_rooms_bed);
                drp_bathrooms.setSelectedValue(currTbl.num_family_bath);
                drp_single_beds.setSelectedValue(currTbl.num_bed_single);
                drp_double_beds.setSelectedValue(currTbl.num_bed_double);
                drp_sofa_beds.setSelectedValue(currTbl.num_sofa_single + currTbl.num_sofa_double);
                drp_cots.setSelectedValue(currTbl.num_cot);
                drp_max_guest.setSelectedValue(currTbl.num_persons_max);
                drp_en_suite.setSelectedValue(currTbl.num_en_suite);
                drp_longue_seats.setSelectedValue(currTbl.num_lounge_seats);
                drp_dining_seats.setSelectedValue(currTbl.num_dining_seats);
                drp_shower_rooms.setSelectedValue(currTbl.num_shower_rooms);
                txt_total_bathrooms.Text = Convert.ToString(currTbl.num_rooms_bath);
                HF_total_bathrooms.Value = Convert.ToString(currTbl.num_rooms_bath);
                //indoor facilities
                chk_cooker.Checked = currTbl.has_cooker == true;
                chk_tv.Checked = currTbl.has_tv == true;
                chk_log_fire.Checked = currTbl.has_fireplace == true;
                chk_friedge.Checked = currTbl.has_fridge == true;
                chk_satelite_tv.Checked = currTbl.has_satelite_tv == true;
                chk_central_heating.Checked = currTbl.has_central_heating == true;
                chk_frezer.Checked = currTbl.has_freezer == true;
                chk_video_player.Checked = currTbl.has_video == true;
                chk_air_condition.Checked = currTbl.has_air_condition == true;
                chk_micro_wave.Checked = currTbl.has_microwave == true;
                chk_dvd.Checked = currTbl.has_dvd == true;
                chk_linen.Checked = currTbl.has_linen == true;
                chk_toaster.Checked = currTbl.has_toaster == true;
                //chk_cd.Checked = _currHoliday.c == true;
                chk_towels.Checked = currTbl.has_towel == true;
                chk_kettle.Checked = currTbl.has_kettle == true;
                chk_internet.Checked = currTbl.has_internet_access == true;
                chk_sauna.Checked = currTbl.has_sauna == true;
                chk_dishwasher.Checked = currTbl.has_dish_washer == true;
                chk_wifi.Checked = currTbl.has_wifi == true;
                chk_gym.Checked = currTbl.has_gym == true;
                chk_cloth_dryer.Checked = currTbl.has_cloth_dryer == true;
                chk_fax.Checked = currTbl.has_fax == true;
                chk_pool_snooker.Checked = currTbl.has_pool_snooker == true;
                chk_iron.Checked = currTbl.has_iron == true;
                chk_hair_dryer.Checked = currTbl.has_hair_dryer == true;
                chk_games_room.Checked = currTbl.has_games_room == true;
                chk_high_chair.Checked = currTbl.has_high_chair == true;
                chk_safe.Checked = currTbl.has_safe == true;
                chk_staffed_property.Checked = currTbl.staffed_property == true;
                chk_washing_machine.Checked = currTbl.has_washing_machine == true;
                chk_telephone.Checked = currTbl.has_telephone == true;
                chk_cd.Checked = currTbl.has_cd == true;
                chk_table_tennis.Checked = currTbl.has_pingpong == true;

                //outdoor facilities
                chk_shared_outoor_heated_pool.Checked = currTbl.has_shared_outdoor_heated == true;
                chk_shared_outoor_unheated_pool.Checked = currTbl.has_shared_outdoor_unheated == true;
                chk_private_outdoor_heated_pool.Checked = currTbl.has_private_outdoor_heated == true;
                chk_private_outdoor_unheated_pool.Checked = currTbl.has_private_outdoor_unheated == true;
                chk_private_indoor_pool.Checked = currTbl.has_pool_private_indoor == true;
                chk_shared_indoor_pool.Checked = currTbl.has_shared_indoor == true;
                chk_children_pool.Checked = currTbl.has_pool_for_children == true;
                chk_hot_tub.Checked = currTbl.has_jacuzzi_hot_tub == true;
                chk_shared_tennis.Checked = currTbl.has_shared_tennis_court == true;
                chk_private_tennis.Checked = currTbl.has_private_tennis_court == true;
                chk_private_garden.Checked = currTbl.has_private_garden == true;
                chk_climbing_frame.Checked = currTbl.has_climbing_frame == true;
                chk_swing_set.Checked = currTbl.has_swwing_set == true;
                chk_trampoline.Checked = currTbl.has_trampoline == true;
                //_currHoliday.has_b = chk_bbq.Checked == true;
                chk_private_fishing_lake.Checked = currTbl.has_private_fishing == true;
                chk_bicycles.Checked = currTbl.has_bicycle == true;
                chk_parking.Checked = currTbl.has_parking == true;
                chk_secure_parking.Checked = currTbl.has_secure_parking == true;
                if (chk_parking.Checked == true)
                    chk_secure_parking.Style.Add("display", "");
                else
                    chk_secure_parking.Style.Add("display", "none");
                chk_roof_terrace.Checked = currTbl.has_solanium_roof_terrace == true;
                chk_balcony_terrace.Checked = currTbl.has_terrace == true;
                chk_boat.Checked = currTbl.has_boat == true;
                chk_seaview.Checked = currTbl.has_sea_view == true;
                chk_shared_garden.Checked = currTbl.has_shared_garden == true;
                chk_bbq.Checked = currTbl.has_barbecue == true;
                //InactiveUnit.HasBalconyOrTerrace = currHoliday.has_terrace; 
                //step3
                chk_short_break.Checked = currTbl.available_shortbreak == 1;
                chk_long_term_lets.Checked = currTbl.available_longlet == 1;
                chk_hen_stag_break.Checked = currTbl.available_henstag == 1;
                chk_corporate_bookings.Checked = currTbl.available_corporate == 1;
                chk_house_swap.Checked = currTbl.available_house_swap == 1;

                drp_chidren.setSelectedValue(currTbl.suitable_children);
                drp_restricted_mobility.setSelectedValue(currTbl.restricted_mobility);
                drp_pets.setSelectedValue(currTbl.allow_pets);
                drp_smokers.setSelectedValue(currTbl.allow_smoking);
                drp_wheelchair_users.setSelectedValue(currTbl.wheelchair_users);

            }
        }
        protected void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null || currTbl.homeId.ToInt64() <= 0)
                {
                    Response.Redirect("EstateChnlHoliday_main.aspx?id=" + IdEstate);
                    return;
                }
                currTbl.homeName = txt_name.Text;
                currTbl.homeSummary = txt_summary.Text;
                currTbl.homeType = drp_property_type.SelectedValue;
                currTbl.homeDescription = txt_unit_description.Text;
                currTbl.num_persons_max = drp_max_guest.getSelectedValueInt();
                currTbl.num_bed_double = drp_double_beds.getSelectedValueInt();
                currTbl.num_bed_single = drp_single_beds.getSelectedValueInt();
                currTbl.num_bed_sofa = drp_sofa_beds.getSelectedValueInt();
                currTbl.num_cot = drp_cots.getSelectedValueInt();
                currTbl.num_dining_seats = drp_dining_seats.getSelectedValueInt();
                currTbl.num_en_suite = drp_en_suite.getSelectedValueInt();
                currTbl.num_shower_rooms = drp_shower_rooms.getSelectedValueInt();
                currTbl.num_family_bath = drp_bathrooms.getSelectedValueInt();
                currTbl.num_rooms_bed = drp_num_bedrooms.getSelectedValueInt();
                currTbl.num_lounge_seats = drp_longue_seats.getSelectedValueInt();
                //_currHoliday.num_rooms_bath = txt_total_bathrooms.Text.objToInt32();
                currTbl.num_rooms_bath = HF_total_bathrooms.Value.objToInt32();
                //indoor facilities
                currTbl.has_cooker = chk_cooker.Checked == true;
                currTbl.has_tv = chk_tv.Checked == true;
                currTbl.has_fireplace = chk_log_fire.Checked == true;
                currTbl.has_fridge = chk_friedge.Checked == true;
                currTbl.has_satelite_tv = chk_satelite_tv.Checked == true;
                currTbl.has_central_heating = chk_central_heating.Checked == true;
                currTbl.has_freezer = chk_frezer.Checked == true;
                currTbl.has_video = chk_video_player.Checked == true;
                currTbl.has_air_condition = chk_air_condition.Checked == true;
                currTbl.has_microwave = chk_micro_wave.Checked == true;
                currTbl.has_dvd = chk_dvd.Checked == true;
                currTbl.has_linen = chk_linen.Checked == true;
                currTbl.has_toaster = chk_toaster.Checked == true;
                //chk_cd.Checked = _currHoliday.c == true;
                currTbl.has_towel = chk_towels.Checked == true;
                currTbl.has_kettle = chk_kettle.Checked == true;
                currTbl.has_internet_access = chk_internet.Checked == true;
                currTbl.has_sauna = chk_sauna.Checked == true;
                currTbl.has_dish_washer = chk_dishwasher.Checked == true;
                currTbl.has_wifi = chk_wifi.Checked == true;
                currTbl.has_gym = chk_gym.Checked == true;
                currTbl.has_cloth_dryer = chk_cloth_dryer.Checked == true;
                currTbl.has_fax = chk_fax.Checked == true;
                currTbl.has_pool_snooker = chk_pool_snooker.Checked == true;
                currTbl.has_iron = chk_iron.Checked == true;
                currTbl.has_hair_dryer = chk_hair_dryer.Checked == true;
                currTbl.has_games_room = chk_games_room.Checked == true;
                currTbl.has_high_chair = chk_high_chair.Checked == true;
                currTbl.has_safe = chk_safe.Checked == true;
                currTbl.staffed_property = chk_staffed_property.Checked == true;
                currTbl.has_cd = chk_cd.Checked == true;
                currTbl.has_washing_machine = chk_washing_machine.Checked == true;
                currTbl.has_telephone = chk_telephone.Checked == true;
                currTbl.has_pingpong = chk_table_tennis.Checked == true;

                //outdoor facilities
                currTbl.has_shared_outdoor_heated = chk_shared_outoor_heated_pool.Checked == true;
                currTbl.has_shared_outdoor_unheated = chk_shared_outoor_unheated_pool.Checked == true;
                currTbl.has_private_outdoor_heated = chk_private_outdoor_heated_pool.Checked == true;
                currTbl.has_private_outdoor_unheated = chk_private_outdoor_unheated_pool.Checked == true;
                currTbl.has_pool_private_indoor = chk_private_indoor_pool.Checked == true;
                currTbl.has_shared_indoor = chk_shared_indoor_pool.Checked == true;
                currTbl.has_pool_for_children = chk_children_pool.Checked == true;
                currTbl.has_jacuzzi_hot_tub = chk_hot_tub.Checked == true;
                currTbl.has_shared_tennis_court = chk_shared_tennis.Checked == true;
                currTbl.has_private_tennis_court = chk_private_tennis.Checked == true;
                currTbl.has_private_garden = chk_private_garden.Checked == true;
                currTbl.has_climbing_frame = chk_climbing_frame.Checked == true;
                currTbl.has_swwing_set = chk_swing_set.Checked == true;
                currTbl.has_trampoline = chk_trampoline.Checked == true;
                currTbl.has_barbecue = chk_bbq.Checked == true;
                currTbl.has_solanium_roof_terrace = chk_roof_terrace.Checked == true;
                currTbl.has_terrace = chk_balcony_terrace.Checked == true;
                currTbl.has_boat = chk_boat.Checked == true;
                currTbl.has_private_fishing = chk_private_fishing_lake.Checked == true;
                currTbl.has_bicycle = chk_bicycles.Checked == true;
                currTbl.has_parking = chk_parking.Checked == true;
                if (chk_parking.Checked == true)
                {
                    currTbl.has_secure_parking = chk_secure_parking.Checked == true;
                }
                else
                    currTbl.has_secure_parking = false;
                currTbl.has_sea_view = chk_seaview.Checked == true;
                currTbl.has_shared_garden = chk_shared_garden.Checked == true;

                //step3
                if (chk_short_break.Checked == true)
                    currTbl.available_shortbreak = 1;
                else
                    currTbl.available_shortbreak = 0;

                if (chk_long_term_lets.Checked == true)
                    currTbl.available_longlet = 1;
                else
                    currTbl.available_longlet = 0;

                if (chk_hen_stag_break.Checked == true)
                    currTbl.available_henstag = 1;
                else
                    currTbl.available_henstag = 0;

                if (chk_corporate_bookings.Checked == true)
                    currTbl.available_corporate = 1;
                else
                    currTbl.available_corporate = 0;

                if (chk_house_swap.Checked == true)
                    currTbl.available_house_swap = 1;
                else
                    currTbl.available_house_swap = 0;

                currTbl.suitable_children = drp_chidren.SelectedValue;
                currTbl.restricted_mobility = drp_restricted_mobility.SelectedValue;
                currTbl.allow_pets = drp_pets.SelectedValue;
                currTbl.allow_smoking = drp_smokers.getSelectedValueInt();
                currTbl.wheelchair_users = drp_wheelchair_users.SelectedValue;
                dc.SaveChanges();
                string errorString = ChnlHolidayUpdate.UpdateUnit_start(IdEstate, App.HOST);
                if (errorString != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione errore!<br />" + errorString.htmlNoWrap() + "\", 340, 110);", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate correttamente.\", 340, 110);", true);

                fillData();
            }
        }

        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }
}