using ModInvoice;
using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class pg_rntEstateDettNew : mainBasePage
    {
        private magaRental_DataContext DC_RENTAL;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public int IdEstate
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

        private RNT_TB_ESTATE TMPcurrEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                if (TMPcurrEstateTB == null)
                    TMPcurrEstateTB = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return TMPcurrEstateTB ?? new RNT_TB_ESTATE();
            }
            set { TMPcurrEstateTB = value; }
        }

        public RNT_LN_ESTATE currEstateLN
        {
            get
            {
                if (TMPlnEstate == null)
                    TMPlnEstate = maga_DataContext.DC_RENTAL.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_lang == CurrentLang.ID);
                return TMPlnEstate ?? new RNT_LN_ESTATE();
            }
            set { TMPlnEstate = value; }
        }
        private RNT_LN_ESTATE TMPlnEstate;

        private LOC_VIEW_ZONE TMPcurrZone;
        public LOC_VIEW_ZONE currZone
        {
            get
            {
                if (TMPcurrZone == null)
                    TMPcurrZone = AppSettings.LOC_ZONEs.SingleOrDefault(x => x.id == currEstateTB.pid_zone && x.pid_lang == CurrentLang.ID);
                return TMPcurrZone ?? new LOC_VIEW_ZONE();
            }
            set { TMPcurrZone = value; }
        }
        protected string IMG_ROOT
        {
            get { return "http://www.rentalinrome.com/"; }
        }

        public int SeasonDateHeaderColSpan = 0;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "pg_estate";
            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "pg_estate_details", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();

            clConfig _config = clUtils.getConfig(CURRENT_SESSION_ID);
            clSearch _ls = _config.lastSearch;
            _config.addTo_myLastVisitedEstateList(PAGE_REF_ID);
            int _dtStartInt = Request.QueryString["dtS"].objToInt32();
            int _dtEndInt = Request.QueryString["dtE"].objToInt32();
            if (_dtStartInt != 0 && _dtEndInt != 0)
            {
                _ls.dtStart = _dtStartInt.JSCal_intToDate();
                _ls.dtEnd = _dtEndInt.JSCal_intToDate();
            }
            clUtils.saveConfig(_config);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                FillData();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setToolTip", "setToolTip();", true);
        }

        private void FillData()
        {
            currEstateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == PAGE_REF_ID && x.is_active == 1 && x.is_deleted != 1);
            if (currEstateTB == null)
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "Apt non attivo id=" + PAGE_REF_ID, _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
                return;
            }

            // details            
            IdEstate = currEstateTB.id;
            ucBooking.IdEstate = IdEstate;

            #region Gallery
            List<RNT_RL_ESTATE_MEDIA> lstAllMedia = maga_DataContext.DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate).ToList();
            List<RNT_RL_ESTATE_MEDIA> lstAllGalleryImages = lstAllMedia.Where(x => x.type == "gallery").OrderBy(x => x.sequence).OrderBy(x => x.sequence).ToList();

            if (lstAllGalleryImages != null && lstAllGalleryImages.Count > 0)
            {
                LV_images.DataSource = lstAllGalleryImages;
                LV_images.DataBind();

                LV_all_images.DataSource = lstAllGalleryImages;
                LV_all_images.DataBind();

                if (lstAllGalleryImages.Count == 1)
                {
                    div_default_image2.Visible = true;
                    div_default_image3.Visible = true;
                    div_default_image4.Visible = true;
                }
                else if (lstAllGalleryImages.Count == 2)
                {
                    div_default_image3.Visible = true;
                    div_default_image4.Visible = true;
                }
                else if (lstAllGalleryImages.Count == 3)
                {
                    div_default_image4.Visible = true;
                }
            }
            else
            {
                div_default_image1.Visible = true;
                div_default_image2.Visible = true;
                div_default_image3.Visible = true;
                div_default_image4.Visible = true;
            }

            List<RNT_RL_ESTATE_MEDIA> lstVideos = lstAllMedia.Where(x => x.type == "video").OrderBy(x => x.sequence).OrderBy(x => x.sequence).ToList();
            if (lstVideos != null && lstVideos.Count > 0)
            {
                LV_Videos.DataSource = lstVideos;
                LV_Videos.DataBind();

            }
            else
                LV_Videos.Visible = false;

            #endregion

            #region extras
            List<int> _configIDs_List = DC_RENTAL.RNT_RL_ESTATE_CONFIG.Where(x => x.pid_estate == IdEstate && x.is_HomeAway == 0).Select(x => x.pid_config).ToList();
            var _configList = DC_RENTAL.RNT_TB_CONFIGs.Where(x => _configIDs_List.Contains(x.id)).ToList();

            int configCnt = CommonUtilities.getSYS_SETTING("amenity_list_cnt").objToInt32();
            if (configCnt == 0) configCnt = 6;

            if (_configList != null && _configList.Count > 0)
            {
                var existingConfigIds = new List<int>();

                var _configList1 = _configList.Take(configCnt).ToList();
                LV_config1.DataSource = _configList1;
                LV_config1.DataBind();
                existingConfigIds.AddRange(_configList1.Select(x => x.id).ToList());

                if (_configList.Count > configCnt)
                {
                    var _configList2 = _configList.Where(x => !existingConfigIds.Contains(x.id)).Take(configCnt).ToList();
                    LV_config2.DataSource = _configList2;
                    LV_config2.DataBind();
                    existingConfigIds.AddRange(_configList2.Select(x => x.id).ToList());
                }

                if (_configList.Count > configCnt * 2)
                {
                    var _configList3 = _configList.Where(x => !existingConfigIds.Contains(x.id)).ToList();
                    LV_config3.DataSource = _configList3;
                    LV_config3.DataBind();
                }
            }
            #endregion

            #region gmap
            PH_mapsContAll.Visible = PH_mapCont.Visible = PH_mapCont_toggler.Visible = ("" + currEstateTB.google_maps).Trim() != "" && currEstateTB.is_google_maps == 1;
            // ltr_gmapPointsScript
            ltr_gmapPointsScript.Text = gmapPointsScript();
            // gmap SV
            PH_gmapSvcont.Visible = PH_gmapSvcont_toggler.Visible = ("" + currEstateTB.sv_coords).Trim() != "" && currEstateTB.is_street_view == 1;
            PH_gmapSvcontNo.Visible = !PH_gmapSvcont.Visible;
            #endregion

            #region comment
            txt_user_email.Attributes.Add("placeholder", contUtils.getLabel("reqEmail"));
            txt_user_comment.Attributes.Add("placeholder", contUtils.getLabel("reqMessage"));
            txt_user_full_name.Attributes.Add("placeholder", contUtils.getLabel("reqFullName"));

            bindDrp(ref drp_voteStaff);
            bindDrp(ref drp_voteService);
            bindDrp(ref drp_voteCleaning);
            bindDrp(ref drp_voteQuality);
            bindDrp(ref drp_votePosition);
            bindDrp(ref drp_voteComfort);
            bindDrp(ref drp_UsertType);
            #endregion

            #region agentcontact
            using (magaUser_DataContext DCUser = new magaUser_DataContext())
            {
                List<USR_RL_ADMIN_LANG> _rlList = maga_DataContext.DC_USER.USR_RL_ADMIN_LANG.Where(x => x.pid_lang == CurrentLang.ID).ToList();
                if (_rlList != null && _rlList.Count > 0)
                {
                    List<int> _agentListLN = _rlList.Select(x => x.pid_admin).ToList();
                    var agents = DCUser.USR_ADMIN.Where(x => x.is_active == 1 && x.isAgentContact == 1 && x.is_deleted != 1 && _agentListLN.Contains(x.id)).ToList();
                    if (agents != null && agents.Count > 0)
                    {
                        List<int> _agentList = agents.Select(x => x.id).ToList();
                        Random r = new Random();
                        int _agent = _agentList.OrderBy(x => r.Next()).FirstOrDefault();
                        var currAgent = agents.FirstOrDefault(x => x.id == _agent);
                        if (currAgent != null)
                        {
                            ltr_userName.Text = currAgent.name + " " + currAgent.surname;
                            HF_agentId.Value = currAgent.id + "";
                            var currAgentLN = _rlList.SingleOrDefault(x => x.pid_admin == currAgent.id);
                            if (currAgentLN != null)
                            {
                                ltr_userImage.Text = "/" + currAgentLN.img_thumb;
                            }
                            PH_agency.Visible = true;
                        }
                        else
                            PH_agency.Visible = false;
                    }
                }
            }
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                var currCompany = dc.dbInvCompanyTBLs.SingleOrDefault(x => x.id == 1);
                if (currCompany != null)
                {
                    ltr_address.Text = currCompany.locAddress + " , " + currCompany.locCity + " , " + currCompany.locState + " , " + currCompany.locCountry;
                    ltr_email.Text = currCompany.contactEmail;
                    ltr_Phone.Text = currCompany.contactPhone;
                }
            }

            txt_email.Attributes.Add("placeholder", contUtils.getLabel("reqEmail"));
            txt_FirstName.Attributes.Add("placeholder", contUtils.getLabel("reqFullName"));
            txt_subject.Attributes.Add("placeholder", contUtils.getLabel("reqSubject"));
            txt_notes.Attributes.Add("placeholder", contUtils.getLabel("reqMessage"));
            #endregion

            #region special offers
            // fill Special Offers
            List<RNT_VIEW_SPECIAL_OFFER> _soList = AppSettings.RNT_VIEW_SPECIAL_OFFERs.Where(x => x.dtPublicStart <= DateTime.Now.Date && x.dtPublicEnd >= DateTime.Now.Date && x.dtEnd > DateTime.Now.AddDays(5) && x.pid_lang == CurrentLang.ID && x.pid_estate == IdEstate && x.is_active == 1).OrderBy(x => x.dtEnd).ToList();
            if (_soList != null && _soList.Count > 0)
            {
                LV_special_offer.DataSource = _soList;
                LV_special_offer.DataBind();
            }
            else
            {
                spOffersCont.Visible = false;
            }
            #endregion

            fillPrice_v1();
        }

        #region Gallery
        protected void LV_images_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlGenericControl div_image = e.Item.FindControl("div_image") as HtmlGenericControl;
                if (div_image == null) return;
                if (e.Item.DisplayIndex > 3)
                {
                    div_image.Style.Add("display", "none");
                }
            }
        }

        protected void LV_all_images_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlGenericControl div_image = e.Item.FindControl("div_image") as HtmlGenericControl;
                div_image.Style.Add("display", (e.Item.DisplayIndex == 4) ? "" : "none");
            }
        }

        protected void LV_Videos_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlGenericControl div_video = e.Item.FindControl("div_video") as HtmlGenericControl;
                div_video.Style.Add("display", (e.Item.DisplayIndex == 0) ? "" : "none");
            }
        }
        #endregion

        protected string gmapPointsScript()
        {
            string _script = "";
            List<LOC_TB_POINT> _list = maga_DataContext.DC_LOCATION.LOC_TB_POINT.Where(x => x.pid_city == AppSettings.RNT_currCity && x.gmaps_coords != null && x.gmaps_coords != "" && x.gmaps_available == 1).ToList();
            List<LOC_TB_POINT_TYPE> _listTypes = maga_DataContext.DC_LOCATION.LOC_TB_POINT_TYPEs.ToList();
            List<LOC_LN_POINT> _listLN = maga_DataContext.DC_LOCATION.LOC_LN_POINTs.ToList();
            foreach (LOC_TB_POINT _point in _list)
            {
                string _img = "images/travelpoints/types/sightseeings.png";
                string _title = "Sightseeings";
                LOC_LN_POINT _ln = _listLN.SingleOrDefault(x => x.pid_lang == CurrentLang.ID && x.pid_point == _point.id);
                if (_ln == null)
                    _ln = _listLN.SingleOrDefault(x => x.pid_lang == 2 && x.pid_point == _point.id);
                if (_ln != null)
                    _title = _ln.title;
                LOC_TB_POINT_TYPE _type = _listTypes.SingleOrDefault(x => x.id == _point.pid_point_type);
                if (_type != null && !string.IsNullOrEmpty(_type.img_preview))
                    _img = _type.img_preview;
                string lat = _point.gmaps_coords.Split(new char[] { '|' })[0].Replace(',', '.');
                string lng = _point.gmaps_coords.Split(new char[] { '|' })[1].Replace(',', '.');
                _script += "_point" + _point.id + " = new google.maps.LatLng(" + lat + ", " + lng + ");";
                _script += "_IconImage" + _point.id + " = new google.maps.MarkerImage('http://www.rentalinrome.com" + CurrentAppSettings.ROOT_PATH + _img + "', new google.maps.Size(20, 20), new google.maps.Point(0, 0), new google.maps.Point(0, 20));";
                _script += "_IconShape" + _point.id + " = { coord: [1, 1, 1, 20, 20, 20, 20, 1], type: 'poly'};";
                _script += "markerOptions" + _point.id + " = { position: _point" + _point.id + ", map: map, icon: _IconImage" + _point.id + " , shape: _IconShape" + _point.id + " };";
                _script += "marker" + _point.id + " = new google.maps.Marker(markerOptions" + _point.id + ");";
                _script += "google.maps.event.addListener(marker" + _point.id + ", 'mouseover', function() { showPointToolTip(marker" + _point.id + ",'" + _title.Replace("'", "\\'") + "','" + _point.img_preview + "');  });\n";
                _script += "google.maps.event.addListener(marker" + _point.id + ", 'mouseout', function() { hidePointToolTip();});\n";
            }
            return _script;
        }

        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem("--" + contUtils.getLabel("lblCountry") + "--", ""));
            //drp_country.Items.Insert(0, new ListItem("- - -", ""));
            if (CurrentLang.ID != 2)
            {
                LOC_LK_COUNTRY _c = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.FirstOrDefault(
                        x => x.inner_notes.ToLower() == CurrentLang.NAME.Substring(3, 2).ToLower());
                if (_c != null)
                    drp_country.setSelectedValue(_c.id.ToString());
            }
        }

        protected void lnk_send_Comment_Click(object sender, EventArgs e)
        {
            SaveComment();
        }

        public void SaveComment()
        {
            dbRntEstateCommentsTBL currTBL = new dbRntEstateCommentsTBL();
            currTBL.cl_country = drp_country.SelectedValue;
            currTBL.cl_email = txt_user_email.Text;
            currTBL.cl_name_full = txt_user_full_name.Text;
            currTBL.body = txt_user_comment.Text.htmlEncode();
            currTBL.cl_name_honorific = "";
            currTBL.cl_pid_lang = CurrentLang.ID;
            currTBL.dtComment = DateTime.Now;
            currTBL.dtCreation = DateTime.Now;
            currTBL.isActive = 0;
            currTBL.pidEstate = IdEstate;
            currTBL.pid_user = 1;
            currTBL.subject = "";
            currTBL.type = "web";
            currTBL.pers = drp_UsertType.SelectedValue;

            currTBL.voteStaff = drp_voteStaff.getSelectedValueInt();
            currTBL.voteService = drp_voteService.getSelectedValueInt();
            currTBL.voteQualityPrice = drp_voteQuality.getSelectedValueInt();
            currTBL.votePosition = drp_votePosition.getSelectedValueInt();

            currTBL.voteComfort = drp_voteComfort.getSelectedValueInt();
            currTBL.voteCleaning = drp_voteCleaning.getSelectedValueInt();

            currTBL.vote = ((currTBL.voteStaff.objToInt32() + currTBL.voteService.objToInt32() + currTBL.voteCleaning.objToInt32() + currTBL.voteComfort.objToInt32() + currTBL.voteQualityPrice.objToInt32() + currTBL.votePosition.objToInt32()) / 6);
            using (DCmodRental dc = new DCmodRental())
            {
                dc.Add(currTBL);
                dc.SaveChanges();
            }
            pnl_Comment_cont.Visible = false;
            pnl_Comment_sent.Visible = true;
            txt_user_comment.Text = "";
            txt_user_email.Text = "";
            txt_user_full_name.Text = "";
        }

        protected void lnk_send_Click(object sender, EventArgs e)
        {
            saveRequest();
        }

        protected void saveRequest()
        {
            bool alternateOld = true;
            RNT_TBL_REQUEST _request = new RNT_TBL_REQUEST();

            _request.pid_lang = CurrentLang.ID;
            string _mailBody = "<table cellspacing=\"0\" cellpadding=\"15\" style=\"font:normal 12px Tahoma, Geneva, sans-serif\">";
            string _row = "";
            _row += "<tr>";
            _row += "<td><strong>Lingua</strong></td>";
            _row += "<td>" + CurrentLang.TITLE + "</td>";
            _row += "</tr>";
            _mailBody += _row;

            _request.name_full = txt_FirstName.Text;
            _mailBody += MailingUtilities.addMailRow("Nome/Cognome", _request.name_full, alternateOld, out alternateOld, false, false, false);

            _request.email = txt_email.Text;
            _mailBody += MailingUtilities.addMailRow("Email", "" + _request.email, alternateOld, out alternateOld, false, false, false);

            _request.request_choice_1 = currEstateTB.id + "";
            _mailBody += MailingUtilities.addMailRow("Preferenze", _request.request_choice_1, alternateOld, out alternateOld, false, false, false);

            #region date and adults
            _request.request_date_start = HF_dtStart.Value.JSCal_stringToDate();
            _mailBody += MailingUtilities.addMailRow("Check-In", "" + _request.request_date_start.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _request.request_date_end = HF_dtEnd.Value.JSCal_stringToDate();
            _mailBody += MailingUtilities.addMailRow("Check-Out", "" + _request.request_date_end.formatITA(true), alternateOld, out alternateOld, false, false, false);
            _request.request_adult_num = int.Parse(HF_num_adult.Value);
            _mailBody += MailingUtilities.addMailRow("num. Adulti", "" + _request.request_adult_num, alternateOld, out alternateOld, false, false, false);
            _request.request_child_num = int.Parse(HF_num_child.Value);
            _mailBody += MailingUtilities.addMailRow("num. Bambini", "" + _request.request_child_num, alternateOld, out alternateOld, false, false, false);
            _request.request_child_num_min = int.Parse(HF_num_infant.Value);
            _mailBody += MailingUtilities.addMailRow("num. Neonati", "" + _request.request_child_num_min, alternateOld, out alternateOld, false, false, false);
            #endregion

            _request.request_notes = txt_notes.Text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            _mailBody += MailingUtilities.addMailRow("Richiesta speciale", "" + _request.request_notes, alternateOld, out alternateOld, false, false, false);
            _request.request_date_created = DateTime.Now;
            _mailBody += MailingUtilities.addMailRow("Data ora creazione", "" + _request.request_date_created, alternateOld, out alternateOld, false, false, false);
            _request.state_date = DateTime.Now;
            _request.state_pid = 1;
            _request.state_subject = "Creata Richiesta";
            _request.state_pid_user = 1;
            _request.request_ip = Request.ServerVariables.Get("REMOTE_HOST");
            _request.pid_creator = 1;

            DC_RENTAL.RNT_TBL_REQUEST.InsertOnSubmit(_request);
            DC_RENTAL.SubmitChanges();
            rntUtils.rntRequest_addState(_request.id, _request.state_pid.Value, _request.state_pid_user.Value, _request.state_subject, "");

            _mailBody += "</table>";
            _request.mail_body = _mailBody;

            _request.pid_related_request = 0;
            _request.pid_operator = HF_agentId.Value.objToInt32();

            string _mSubject = txt_subject.Text;
            _request.request_subject = _mSubject;

            _request.operator_date = DateTime.Now;
            string _mailSend = "";

            var currAgent = maga_DataContext.DC_USER.USR_ADMIN.SingleOrDefault(x => x.id == HF_agentId.Value.objToInt32());
            if (currAgent == null) currAgent = new USR_ADMIN();

            if (MailingUtilities.autoSendMailTo(_mSubject, _mailBody, currAgent.email, false, "Agente Contatti Richiesta"))
                _mailSend = "Assegnato e inviato mail a " + currAgent.name + " (" + currAgent.email + ")";
            else
                _mailSend = "Assegnato " + currAgent.name + " - Errore nel invio mail a (" + currAgent.email + ")";

            rntUtils.rntRequest_addState(_request.id, 0, _request.state_pid_user.Value, _mailSend, _mailBody);
            _mailBody = _mailSend + "<br/><br/>" + _mailBody;

            DC_RENTAL.SubmitChanges();
            pnl_request_cont.Visible = false;
            pnl_request_sent.Visible = true;
            rntUtils.rntRequest_mailNewCreation(_request);
            MailingUtilities.autoSendMailTo(_mSubject, _mailBody, MailingUtilities.ADMIN_MAIL, false, "Agent Contact Request al admin");
        }

        protected void bindDrp(ref DropDownList drp)
        {
            drp.Items.Clear();
            if (drp.ID == "drp_UsertType")
            {
                drp.Items.Add(new ListItem(contUtils.getLabel("lblIam"), ""));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblMan"), "m"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblWoman"), "f"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblCouple"), "co"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblFamily"), "fm"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblGroups"), "gr"));
            }
            else
            {

                if (drp.ID == "drp_voteStaff") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForStaff") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_voteService") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForService") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_voteCleaning") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForCleaning") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_voteQuality") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForQualityPrice") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_votePosition") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForPosition") + " " + contUtils.getLabel("lblRating"), ""));
                else if (drp.ID == "drp_voteComfort") drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingForComfort") + " " + contUtils.getLabel("lblRating"), ""));

                drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingCarente"), "4"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingSufficiente"), "6"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingBuono"), "8"));
                drp.Items.Add(new ListItem(contUtils.getLabel("lblRatingOttimo"), "10"));
            }
        }

        protected void fillPrice_v1()
        {
            PH_priceListCont_v1.Visible = true;
            PH_priceListCont_v2.Visible = false;
            decimal _pr_discount7daysApply = 1 - (currEstateTB.pr_discount7days.objToDecimal() / 100);
            decimal _pr_discount30daysApply = 1 - (currEstateTB.pr_discount30days.objToDecimal() / 100);
            decimal _minPrice = rntUtils.rntEstate_minPrice(IdEstate);
            ltr_minPriceDay.Text = _minPrice.ToString("N2");
            decimal _tmpPriceWeek = _minPrice * 7 * _pr_discount7daysApply;
            _tmpPriceWeek = _tmpPriceWeek.customRound(true);
            ltr_minPriceWeek.Text = _tmpPriceWeek.ToString("N2");
            string _priceDetails = "";
            decimal _prTemp = 0;
            int pr_basePersons = currEstateTB.pr_basePersons.objToInt32();


            string strScripthidePriceCol = "function setPriceColumns(){";
            // tabella delle stagioni
            var seasonGroup = currEstateTB.pidSeasonGroup.objToInt32();
            var seasonDateList = new List<dbRntSeasonDatesTBL>();
            using (DCmodRental dc = new DCmodRental())
                seasonDateList = dc.dbRntSeasonDatesTBLs.Where(x => x.dtEnd >= DateTime.Now && x.dtStart <= DateTime.Now.AddYears(1) && x.pidSeasonGroup == seasonGroup).OrderBy(x => x.dtStart).ToList();
            LVseasonDates.DataSource = seasonDateList;
            LVseasonDates.DataBind();

            if (seasonDateList.Any(x => x.pidPeriod == 1))
            {
                LVseasonDates_1.DataSource = seasonDateList.Where(x => x.pidPeriod == 1);
                LVseasonDates_1.DataBind();
                LVseasonDates_1.Visible = true;
                th_datePeriod1.Visible = true;
                td_colPeriod1.Visible = true;
                th_PricePeriod1.Visible = true;
                //showPeriod_1_Price = true;
                td_day_1.Visible = true;
                td_week_1.Visible = true;

                SeasonDateHeaderColSpan = SeasonDateHeaderColSpan + 1;
                //td_pr_1.Visible = true;
                //  td_pr_1_w.Visible = true;

            }
            else
                strScripthidePriceCol += "$(\".td_pr_1\").hide();" + "$(\".td_pr_1_w\").hide();";

            if (seasonDateList.Any(x => x.pidPeriod == 2))
            {
                LVseasonDates_2.DataSource = seasonDateList.Where(x => x.pidPeriod == 2);
                LVseasonDates_2.DataBind();
                th_datePeriod2.Visible = true;
                td_colPeriod2.Visible = true;
                th_PricePeriod2.Visible = true;

                // showPeriod_2_Price = true;
                td_day_2.Visible = true;
                td_week_2.Visible = true;
                //td_pr_2.Visible = true;
                // td_pr_2_w.Visible = true;
                SeasonDateHeaderColSpan = SeasonDateHeaderColSpan + 1;
            }
            else
                strScripthidePriceCol += "$(\".td_pr_2\").hide();" + "$(\".td_pr_2_w\").hide();";

            if (seasonDateList.Any(x => x.pidPeriod == 3))
            {
                LVseasonDates_3.DataSource = seasonDateList.Where(x => x.pidPeriod == 3);
                LVseasonDates_3.DataBind();
                th_datePeriod3.Visible = true;
                td_colPeriod3.Visible = true;
                th_PricePeriod3.Visible = true;

                td_day_3.Visible = true;
                td_week_3.Visible = true;
                SeasonDateHeaderColSpan = SeasonDateHeaderColSpan + 1;
                //  td_pr_3.Visible = true;


                // td_pr_3_w.Visible = true;
            }
            else
                strScripthidePriceCol += "$(\".td_pr_3\").hide();" + "$(\".td_pr_3_w\").hide();";

            if (seasonDateList.Any(x => x.pidPeriod == 4))
            {
                LVseasonDates_4.DataSource = seasonDateList.Where(x => x.pidPeriod == 4);
                LVseasonDates_4.DataBind();
                th_datePeriod4.Visible = true;
                td_colPeriod4.Visible = true;
                th_PricePeriod4.Visible = true;

                td_day_4.Visible = true;
                td_week_4.Visible = true;
                SeasonDateHeaderColSpan = SeasonDateHeaderColSpan + 1;
                //   td_pr_4.Visible = true;
                //  td_pr_4_w.Visible = true;
            }
            else
                strScripthidePriceCol += "$(\".td_pr_4\").hide();" + "$(\".td_pr_4_w\").hide();";

            for (int i = pr_basePersons; i <= currEstateTB.num_persons_max.objToInt32(); i++)
            {
                int extraPersons = i - pr_basePersons;
                string numPers_string = extraPersons == 0 && currEstateTB.num_persons_min.objToInt32() < pr_basePersons ? i + " " + CurrentSource.getSysLangValue("lblPax") + " or less" : i + " " + CurrentSource.getSysLangValue("lblPax");
                string _prStr = ltr_priceTemplate.Text.Replace("#num_pers#", numPers_string);

                // low
                _prTemp = currEstateTB.pr_1_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_1_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_1#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_1_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");

                // Medium
                _prTemp = currEstateTB.pr_4_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_4_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_4#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_4_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");

                // hight
                _prTemp = currEstateTB.pr_2_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_2_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_2#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_2_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                // very hight
                _prTemp = currEstateTB.pr_3_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_3_opt.objToDecimal());
                _prStr = _prStr.Replace("#pr_3#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _prTemp = _prTemp * 7 * _pr_discount7daysApply;
                _prTemp = _prTemp.customRound(true);
                _prStr = _prStr.Replace("#pr_3_w#", _prTemp != 0 ? "&euro; " + _prTemp.ToString("N2") : "<span class=\"onreq\">" + CurrentSource.getSysLangValue("lblOnRequest") + "</span>");
                _priceDetails += _prStr;
            }
            ltr_priceDetails.Text = _priceDetails;
            strScripthidePriceCol += "}";
            ltr_hidePriceColScript.Text = strScripthidePriceCol;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setPriceColumns", "setPriceColumns();", true);

        }
    }
}