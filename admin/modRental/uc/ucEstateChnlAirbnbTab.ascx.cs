using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;
using System.IO;

namespace MagaRentalCE.admin.modRental.uc
{
    public partial class ucEstateChnlAirbnbTab : System.Web.UI.UserControl
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
                displayLinks();
                fillCancelltionPolicy();
                fillHosts();
            }
        }
        protected void fillHosts()
        {
            magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
            var hostIds = CommonUtilities.getSYS_SETTING("airbnb_host_ids").splitStringToList(",");
            var hosts = DC_AIRBNB.RntChnlAirbnbAuthenticationCode.Where(x => x.hostId != "" && hostIds.Contains(x.hostId)).ToList();
            foreach (var host in hosts)
            {
                drp_host.Items.Add(new ListItem(host.hostId + " - " + host.name + "", host.hostId + ""));
            }
            drp_host.Items.Insert(0, new ListItem("----", "0"));
        }
        protected void displayLinks()
        {
            magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;

            var currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currEstate == null) return;


            using (DCmodRental dc = new DCmodRental())
            {
                dbRntChnlAirbnbEstateTBL objAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == IdEstate);
                if (objAirbnbEstate == null || (objAirbnbEstate != null && objAirbnbEstate.hostId + "" == ""))
                {
                    tabsAirbnb0.Visible = true;
                    lnk_send_content.Visible = false;
                    tabsAirbnb1.Visible = false;
                    ltr_host.Text = " - ";
                }
                else if (objAirbnbEstate != null && objAirbnbEstate.hostId + "" != "")
                {
                    tabsAirbnb0.Visible = true;
                    lnk_send_content.Visible = true;
                    tabsAirbnb1.Visible = true;

                    ltr_host.Text = objAirbnbEstate.hostId;
                    drp_host.setSelectedValue(objAirbnbEstate.hostId);
                }

                if (objAirbnbEstate == null || (objAirbnbEstate != null && objAirbnbEstate.airbnb_id + "" == ""))
                {
                    ltr_url.Text = " - ";
                    ltr_airbnb_id.Text = " - ";
                }
                else
                {
                    ltr_url.Text = "https://it.airbnb.ch/rooms/" + objAirbnbEstate.airbnb_id;
                    ltr_airbnb_id.Text = objAirbnbEstate.airbnb_id + "";

                }
                if (objAirbnbEstate == null || (objAirbnbEstate != null && objAirbnbEstate.isSentBasicDetails.objToInt32() == 0 && objAirbnbEstate.isExisting.objToInt32() == 0))
                {
                    //lnk_send_content.Visible = true;
                    lnk_send_description.Visible = false;
                    lnk_send_image.Visible = false;
                    lnk_delete_imges.Visible = false;
                    lnk_delete_listing.Visible = false;
                    lnk_send_availability.Visible = false;
                    lnk_Send_los_prices.Visible = false;
                    tbl_update.Visible = false;
                    ltr_status.Text = "new";
                    ltr_sync_category.Text = "<span style ='color:red'>" + contUtils.getLabel("lblNo") + "</span>";
                    ltr_listed.Text = " - ";
                    ltr_booking_lead_time.Text = "0";
                    ltr_allow_request_to_book.Text = contUtils.getLabel("lblNo");
                    tabsAirbnb3.Visible = false;
                    lnk_send_prices.Visible = false;
                    ltr_cancellation_policy.Text = " - ";
                    tabsAirbnb4.Visible = false;
                    ltr_checkin_time.Text = " - ";
                    ltr_checkout_time.Text = " - ";
                    //ltr_url.Text = " - ";
                    //ltr_airbnb_id.Text = " - ";
                }
                else
                {
                    tabsAirbnb0.Visible = false;
                    lnk_send_image.Visible = true;
                    lnk_delete_imges.Visible = true;
                    lnk_send_description.Visible = true;
                    lnk_send_content.Visible = false;
                    lnk_Send_los_prices.Visible = false;
                    tbl_update.Visible = true;
                    ltr_status.Text = objAirbnbEstate.status + "" != "" ? objAirbnbEstate.status : "new";

                    if (objAirbnbEstate.syncCategory+"" == "")
                    {
                        ltr_sync_category.Text = "<span style ='color:red'>" + contUtils.getLabel("lblNo") + "</span> - sync_undecided";
                    }
                    else
                    {
                        string sync_cat = "";
                        if (objAirbnbEstate.syncCategory == "sync_all")
                        {
                            sync_cat = "Synchronize All";
                        }
                        else if (objAirbnbEstate.syncCategory == "sync_rates_and_availability")
                        {
                            sync_cat = "Synchronize only rates and availability";
                        }
                        else if (objAirbnbEstate.syncCategory == "null")
                        {
                            sync_cat = "Not Linked";
                        }

                        ltr_sync_category.Text = "<span style ='color:Green'>" + contUtils.getLabel("lblYes") + "</span> - " + sync_cat;
                    }

                    ltr_listed.Text = objAirbnbEstate.hasAvailbility.objToInt32() == 1 ? "listed" : "Unlisted";
                    ltr_booking_lead_time.Text = objAirbnbEstate.hours.objToInt32() + "";
                    ltr_allow_request_to_book.Text = objAirbnbEstate.isAllowRequest.objToInt32() == 0 ? "No" : "Yes";
                    tabsAirbnb3.Visible = true;
                    lnk_send_prices.Visible = true;
                    lnk_send_availability.Visible = true;
                    tabsAirbnb4.Visible = true;
                    ltr_listed.Text = objAirbnbEstate.hasAvailbility.objToInt32() == 1 ? "Listed" : "Unlisted";
                    ltr_cancellation_policy.Text = objAirbnbEstate.cancelltionPolicy + "" == "" ? " - " : objAirbnbEstate.cancelltionPolicy + "";
                    ltr_checkin_time.Text = objAirbnbEstate.checkinStartTime + "" == "" ? " - " : objAirbnbEstate.checkinStartTime + "";
                    ltr_checkin_end_time.Text = objAirbnbEstate.checkinEndTime + "" == "" ? " - " : objAirbnbEstate.checkinEndTime + "";
                    ltr_checkout_time.Text = objAirbnbEstate.checkOutTime + "" == "" ? " - " : objAirbnbEstate.checkOutTime + "";


                    //set values by default so not change in changing other values
                    drp_allow_request_to_book.setSelectedValue(objAirbnbEstate.isAllowRequest);
                    drp_lead_time.setSelectedValue(objAirbnbEstate.hours + "");

                    drp_cancelltion_policy.setSelectedValue(objAirbnbEstate.cancelltionPolicy);
                    drp_checkin_start_time.setSelectedValue(objAirbnbEstate.checkinStartTime);
                    drp_checkin_end_time.setSelectedValue(objAirbnbEstate.checkinEndTime);
                    drp_checkout_time.setSelectedValue(objAirbnbEstate.checkOutTime);
                }

                for (int i = 0; i <= 24; i++)
                {
                    drp_lead_time.Items.Add(new ListItem(i + "", i + ""));
                }
                drp_lead_time.Items.Add(new ListItem("48", "48"));
                drp_lead_time.Items.Add(new ListItem("72", "72"));
                drp_lead_time.Items.Add(new ListItem("168", "168"));



                //if (currPropertyHost != null)
                //{
                //    ltr_host.Text = currPropertyHost.hostId;
                //    drp_host.setSelectedValue(currPropertyHost.hostId);
                //}

                //if (currEstate != null && currEstate.priceVersion == 6)
                //    lnk_Send_los_prices.Visible = true;
                //else
                    lnk_Send_los_prices.Visible = false;
                //magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;

            }
        }
        protected void displayStatus()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntChnlAirbnbEstateTBL objAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == IdEstate);
                if (objAirbnbEstate == null)
                {
                    ltr_status.Text = "new";
                    ltr_sync_category.Text = "Synchronization Undecided";
                    ltr_listed.Text = "Unlisted";
                    ltr_cancellation_policy.Text = "";
                }
                else
                {
                    ltr_sync_category.Text = objAirbnbEstate.syncCategory + "" != "" ? objAirbnbEstate.syncCategory : "Synchronization Undecided";
                    ltr_status.Text = objAirbnbEstate.status + "" != "" ? objAirbnbEstate.status : "new";
                    if (ltr_sync_category.Text == "sync_all")
                    {
                        ltr_sync_category.Text = "Synchronize All";
                    }
                    else if (ltr_sync_category.Text == "sync_rates_and_availability")
                    {
                        ltr_sync_category.Text = "Synchronize only rates and availability";
                    }
                    else if (ltr_sync_category.Text == "null")
                    {
                        ltr_sync_category.Text = "Not Linked";
                    }
                    ltr_listed.Text = objAirbnbEstate.hasAvailbility.objToInt32() == 1 ? "Listed" : "Unlisted";
                    ltr_booking_lead_time.Text = objAirbnbEstate.hours.objToInt32() + "";
                    ltr_allow_request_to_book.Text = objAirbnbEstate.isAllowRequest.objToInt32() == 0 ? "No" : "Yes";
                    ltr_cancellation_policy.Text = objAirbnbEstate.cancelltionPolicy + "" == "" ? " - " : objAirbnbEstate.cancelltionPolicy + "";

                    var currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    //if (currEstate != null && currEstate.priceVersion == 6)
                    //    lnk_Send_los_prices.Visible = true;
                    //else
                        lnk_Send_los_prices.Visible = false;
                }
            }
        }
        protected void fillCancelltionPolicy()
        {
            drp_cancelltion_policy.Items.Add(new ListItem("flexible", "flexible"));
            drp_cancelltion_policy.Items.Add(new ListItem("moderate", "moderate"));
            drp_cancelltion_policy.Items.Add(new ListItem("strict_14_with_grace_period", "strict_14_with_grace_period"));
            drp_cancelltion_policy.Items.Add(new ListItem("strict", "strict"));
            drp_cancelltion_policy.Items.Add(new ListItem("super_strict_30", "super_strict_30"));
            drp_cancelltion_policy.Items.Add(new ListItem("super_strict_60", "super_strict_60"));

            //checkin/checkout time
            drp_checkin_start_time.Items.Add(new ListItem("FLEXIBLE", "FLEXIBLE"));
            drp_checkin_end_time.Items.Add(new ListItem("FLEXIBLE", "FLEXIBLE"));
            drp_checkout_time.Items.Add(new ListItem("FLEXIBLE", "FLEXIBLE"));

            for (int i = 0; i <= 23; i++)
            {
                drp_checkin_start_time.Items.Add(new ListItem(i + "", i + ""));
                drp_checkin_end_time.Items.Add(new ListItem(i + "", i + ""));
                drp_checkout_time.Items.Add(new ListItem(i + "", i + ""));
            }

            var currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currEstate != null)
            {
                if (currEstate.pid_country == 108)
                {
                    drp_cancelltion_policy.Items.Add(new ListItem("flexible_new", "flexible_new"));
                    drp_cancelltion_policy.Items.Add(new ListItem("moderate_new", "moderate_new"));
                    drp_cancelltion_policy.Items.Add(new ListItem("strict_14_with_grace_period_new", "strict_14_with_grace_period_new"));
                    drp_cancelltion_policy.Items.Add(new ListItem("strict_new", "strict_new"));
                    drp_cancelltion_policy.Items.Add(new ListItem("super_strict_30_new", "super_strict_30_new"));
                    drp_cancelltion_policy.Items.Add(new ListItem("super_strict_60_new", "super_strict_60_new"));
                }

                //drp_checkin_start_time.setSelectedValue(currEstate.Def_CheckIn.Replace(":00:00", ""));
                //drp_checkout_time.setSelectedValue(currEstate.Def_CheckOut.Replace(":00:00", ""));
            }
        }
        protected void lnk_send_content_Click(object sender, EventArgs e)
        {
            RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
            if (currEstate == null)
            {
                return;
            }

            List<string> amenities = new List<string>();
            using (DCmodRental dc = new DCmodRental())
            {
                var extrasList = dc.dbRntEstateExtrasRLs.Where(x => x.pidEstate == currEstate.id).ToList();
                foreach (var extra in extrasList)
                {
                    var currAirbnbExtra = dc.dbRntChnlAirbnbLkAmenityTBLs.SingleOrDefault(x => x.refId == extra.pidEstateExtras + "");
                    if (currAirbnbExtra != null)
                    {
                        amenities.Add(currAirbnbExtra.code);
                    }
                }
            }

            string str = "";
            string sep = "";
            string error = "";

            if (currEstate.code == "")
            {
                str = sep + "Name";
                sep = " ,";
            }
            if (currEstate.pid_city.objToInt32() == 0)
            {
                str += sep + "City";
                sep = " ,";
            }
            if (currEstate.pid_country.objToInt32() == 0)
            {
                str += sep + "Country";
                sep = " ,";
            }
            if (string.IsNullOrEmpty(currEstate.google_maps) || ((currEstate.google_maps + "") != "" && currEstate.google_maps.Split(',').Length < 1))
            {
                str += sep + "Lattitude and Longitude";
                sep = " ,";
            }
            if (rntUtils.rntEstate_minPrice(currEstate.id).objToInt32() == 0)
            {
                str += sep + "Price";
                sep = " ,";
            }
            if (amenities.Count < 5)
            {
                str += sep + "at least 5 amenities";
                sep = " ,";
            }
            #region propertyType
            //using (DCchnlExpedia dcChnlExpedia = new DCchnlExpedia())
            //using (DCmodRental dcRnt = new DCmodRental())
            //{
            //    var propertyCategory = dcChnlExpedia.dbRntChnlExpediaPropertyTypeRLs.SingleOrDefault(x => x.pidCategory == currEstate.pid_category.objToInt32());
            //    if (propertyCategory == null)
            //    {
            //        str += sep + "Property type";
            //        sep = " ,";
            //    }
            //}
            #endregion
            if (str != "")
            {
                error += "Please add " + str;
            }
            if (error == "")
            {
                ChnlAirbnbUpdate.PropertyNew_start(currEstate);
                displayLinks();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert11", "radalert(\"Dat send in progress...\" , 340, 110);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert('" + error + "', 340, 110);", true);
                return;
            }
        }

        protected void lnk_send_description_Click(object sender, EventArgs e)
        {
            string str = "";
            string sep = "";
            int descriptionLength = 0;

            RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
            if (currEstate == null)
            {
                return;
            }
            RNT_LN_ESTATE currLNEstate = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == App.DefLangID);
            if (currLNEstate == null)
            {
                return;
            }
            if (currLNEstate.title.Length < 8)
            {
                str = "Please add title with at least 8 characters";
                sep = " ,";
            }

            descriptionLength = (currLNEstate.description + "").Length;
            //descriptionLength = descriptionLength + (currLNEstate.villaDiretionsDescription + "").Length;
            //escriptionLength = descriptionLength + (currLNEstate.nbh_description + "").Length;
            //descriptionLength = descriptionLength + (currLNEstate.extraNotes + "").Length;

            if (descriptionLength < 50)
            {
                str += sep + " Please add description(includes summary ,zone descrition ,extra notes and How to reach the structure)  with at least 50 characters";
            }

            if (str != "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert4", "radalert('" + str + "', 340, 110);", true);
                return;
            }
            else
            {
                ChnlAirbnbUpdate.PropertyDescription_start(IdEstate);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert12", "radalert(\"Data send in progress...\" , 340, 110);", true);
            }
        }

        protected void lnk_send_image_Click(object sender, EventArgs e)
        {
            int imgCount = 0;
            RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1 && x.is_deleted != 1);
            if (currEstate == null)
            {
                return;
            }
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntChnlAirbnbEstateTBL objAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == IdEstate);
                if (objAirbnbEstate == null)
                {
                    ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                    ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                    return;
                }
                if (objAirbnbEstate.isSentImages.objToInt32() == 0)
                {
                    var images = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == currEstate.id && x.type == "original").ToList();
                    if (images.Count < 7)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert1", "radalert(\"please add at least 7 images.\", 340, 110);", true);
                        return;
                    }
                    else
                    {
                        foreach (var image in images)
                        {
                            string path = Path.Combine(App.SRP, image.img_banner);
                            int height = System.Drawing.Image.FromFile(path).Height;
                            int width = System.Drawing.Image.FromFile(path).Width;

                            if (width > 800 && height > 500)
                            {
                                imgCount = imgCount + 1;
                            }

                        }

                        if (imgCount < 3)
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert2", "radalert(\"please add at least 3 images with high-resolution (800x500 pixels).\" , 340, 110);", true);
                            return;
                        }
                    }
                }
            }
            ChnlAirbnbUpdate.PropertyImage_start(IdEstate);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert13", "radalert(\"Data send in progress...\" , 340, 110);", true);
        }

        protected void lnk_update_details_Click(object sender, EventArgs e)
        {
            if (drp_status.SelectedValue == "ready for review")
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    dbRntChnlAirbnbEstateTBL objAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == IdEstate);
                    if (objAirbnbEstate == null)
                    {
                        ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                        ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                        return;
                    }
                    if (objAirbnbEstate.isSentBasicDetails == 1 && objAirbnbEstate.isSentDescription == 1 && objAirbnbEstate.isSentImages == 1)
                    {
                        ChnlAirbnbUpdate.PropertyUpdate_start(IdEstate, drp_status.SelectedValue);
                    }
                    else
                    {
                        string str = "";
                        string sep = "";

                        if (objAirbnbEstate.isSentBasicDetails.objToInt32() == 0)
                        {
                            str = "basic details";
                            sep = " ,";
                        }

                        if (objAirbnbEstate.isSentDescription.objToInt32() == 0)
                        {
                            str += sep + "description";
                            sep = " ,";
                        }

                        if (objAirbnbEstate.isSentImages.objToInt32() == 0)
                        {
                            str += sep + "images";
                            sep = " ,";
                        }

                        string error = "Please send " + str;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert3", "radalert('" + error + "', 340, 110);", true);
                        return;
                    }
                }
            }
            else
            {
                ChnlAirbnbUpdate.PropertyUpdate_start(IdEstate, drp_status.SelectedValue);
                displayStatus();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert14", "radalert(\"Updating Data in progress...\" , 340, 110);", true);
            }
        }

        protected void lnk_update_sync_category_Click(object sender, EventArgs e)
        {
            ChnlAirbnbUpdate.PropertyUpdateSync_start(IdEstate, drp_sync_category.SelectedValue);
            displayStatus();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert14", "radalert(\"Updating Synchronization category in progress...\" , 340, 110);", true);
        }

        protected void lnk_unlist_listng_Click(object sender, EventArgs e)
        {
            ChnlAirbnbUpdate.PropertyUpdateUnlist_process_start(IdEstate, Convert.ToBoolean(drp_unlist_listing.SelectedValue));
            displayStatus();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert15", "radalert(\"Updating listing/re-listing listing in progress...\" , 340, 110);", true);
        }

        protected void lnk_send_data_Click(object sender, EventArgs e)
        {
            ChnlAirbnbUpdate.PropertyLeadTime_start(IdEstate, drp_lead_time.getSelectedValueInt(), drp_allow_request_to_book.getSelectedValueInt());
            displayStatus();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert16", "radalert(\"Updating Booking lead time in progress...\" , 340, 110);", true);
        }

        protected void lnk_send_availability_Click(object sender, EventArgs e)
        {
            ChnlAirbnbUpdate.UpdateAvailabilityUpdate_start(IdEstate);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert16", "radalert(\"Sending Availbility/inventory in progress...\" , 340, 110);", true);

        }

        protected void lnk_delete_listing_Click(object sender, EventArgs e)
        {
            ChnlAirbnbUpdate.PropertyDelete_process_start(IdEstate);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert17", "radalert(\"Deleting listing in progress...\" , 340, 110);", true);
            displayLinks();
        }

        protected void lnk_send_prices_Click(object sender, EventArgs e)
        {
            //ChnlAirbnbUpdate.PropertyUpdateCurrency_process_start(IdEstate);
            ChnlAirbnbUpdate.UpdateRates_start(IdEstate);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert18", "radalert(\"Sending Prices in progress...\" , 340, 110);", true);

        }

        protected void lnk_send_cancelltion_policy_Click(object sender, EventArgs e)
        {
            ChnlAirbnbUpdate.PropertyUpdatePolicy_process_start(IdEstate, drp_cancelltion_policy.SelectedValue, drp_checkin_start_time.SelectedValue, drp_checkin_end_time.SelectedValue, drp_checkout_time.SelectedValue);
            displayStatus();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert19", "radalert(\"Updating Cancellation Policy in progress...\" , 340, 110);", true);

        }

        protected void lnk_Send_los_prices_Click(object sender, EventArgs e)
        {
            ChnlAirbnbUpdate.UpdateRatesLOS_start(IdEstate);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert18", "radalert(\"Sending LOS Prices in progress...\" , 340, 110);", true);

        }

        protected void lnk_update_host_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == IdEstate);
                if (currAirbnbEstate == null)
                {
                    currAirbnbEstate = new dbRntChnlAirbnbEstateTBL();
                    currAirbnbEstate.mr_id = IdEstate;
                    dc.Add(currAirbnbEstate);
                    dc.SaveChanges();
                }

                magaChnlAirbnbDataContext DC_AIRBNB = maga_DataContext.DC_AIRBNB;
                var currPropertyHost = DC_AIRBNB.RntChnlAirbnbPropertyHostRL.FirstOrDefault(x => x.pidEstate == IdEstate && x.hostId == currAirbnbEstate.hostId + "");
                if (currAirbnbEstate.hostId + "" == "")
                {
                    currPropertyHost = new RntChnlAirbnbPropertyHostRL();
                    currPropertyHost.pidEstate = IdEstate;
                    currPropertyHost.hostId = drp_host.SelectedValue;
                    DC_AIRBNB.RntChnlAirbnbPropertyHostRL.InsertOnSubmit(currPropertyHost);
                }
                else
                {
                    currPropertyHost.hostId = drp_host.SelectedValue;
                    DC_AIRBNB.SubmitChanges();
                }
                currAirbnbEstate.hostId = drp_host.SelectedValue;
                dc.SaveChanges();
                DC_AIRBNB.SubmitChanges();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert19", "radalert(\"Updated Host.\" , 340, 110);", true);
                displayLinks();
            }
        }

        protected void lnk_update_id_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var currAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == IdEstate);
                if (currAirbnbEstate == null)
                {
                    currAirbnbEstate = new dbRntChnlAirbnbEstateTBL();
                    currAirbnbEstate.mr_id = IdEstate;
                    dc.Add(currAirbnbEstate);
                    dc.SaveChanges();
                }
                currAirbnbEstate.airbnb_id = txt_airbnb_id.Text.Trim().objToInt32();
                currAirbnbEstate.isExisting = 1;
                dc.SaveChanges();
                displayLinks();
                txt_airbnb_id.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert18", "radalert(\"Airbnb ID connected successfully.\" , 340, 110);", true);

            }
        }
        protected void lnk_delete_imges_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntChnlAirbnbEstateTBL objAirbnbEstate = dc.dbRntChnlAirbnbEstateTBLs.SingleOrDefault(x => x.mr_id == IdEstate);
                if (objAirbnbEstate == null)
                {
                    ChnlAirbnbUtils.addLog("ERROR", "Missing Airbnb ID", "", "", "");
                    ErrorLog.addLog("", "Airbnb description", "Missing Airbnb ID");
                    return;
                }
                if (objAirbnbEstate.isSentImages.objToInt32() == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert20", "radalert(\"please add before images.\", 340, 110);", true);
                    return;
                }
            }
            ChnlAirbnbUpdate.PropertyImageDelete_start(IdEstate);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert20", "radalert(\"Images deletion in progress.\", 340, 110);", true);
        }
    }
}
