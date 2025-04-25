using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.pdfgenerator
{
    public partial class pdf_rnt_reservation_voucherfo : mainBasePage
    {
        public long IdReservation
        {
            get
            {
                return HF_IdReservation.Value.ToInt64();
            }
            set
            {
                HF_IdReservation.Value = value.ToString();
            }
        }
        public RNT_TBL_RESERVATION tblReservation
        {
            get
            {
                if (_currReservation == null)
                    _currReservation = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                return _currReservation ?? new RNT_TBL_RESERVATION();
            }
        }
        private RNT_TBL_RESERVATION _currReservation;
        public USR_TBL_CLIENT tblClient
        {
            get
            {
                if (_currClient == null)
                    _currClient = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == tblReservation.cl_id);
                if (_currClient == null && tblReservation.agentClientID.objToInt64() > 0)
                    _currClient = getClientFromAgent(tblReservation.agentClientID.objToInt64());
                return _currClient ?? new USR_TBL_CLIENT();
            }
        }
        private USR_TBL_CLIENT _currClient;
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
        public RNT_TB_ESTATE tblEstate
        {
            get
            {
                if (_currEstate == null)
                    _currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return _currEstate ?? new RNT_TB_ESTATE();
            }
        }
        private RNT_TB_ESTATE _currEstate;
        public dbRntAgentTBL agentTbl
        {
            get
            {
                if (TMPagentTbl == null)
                    using (DCmodRental dc = new DCmodRental())
                        TMPagentTbl = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == tblReservation.agentID);
                return TMPagentTbl;
            }
        }
        private dbRntAgentTBL TMPagentTbl;
        public int _currLang
        {
            get
            {
                return HF_pid_lang.Value.ToInt32();
            }
            set
            {
                HF_pid_lang.Value = value.ToString();
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private magaUser_DataContext DC_USER;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_USER = maga_DataContext.DC_USER;
            if (!IsPostBack)
            {
                if (Request.QueryString["uid"] != null)
                {
                    _currReservation = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.unique_id != null && x.uid_2 != null && x.unique_id.ToString().ToLower() + "" + x.uid_2.ToString().ToLower() == Request.QueryString["uid"].ToLower());
                    if (_currReservation != null)
                        IdReservation = _currReservation.id;
                }
                else
                { 
                    IdReservation = Request.QueryString["id"].objToInt64();
                    if (IdReservation > 151300)
                    {
                        IdReservation = 0;
                    }
                }
                fillData();
            }
        }
        public void fillData()
        {
            
            _currReservation = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (_currReservation == null)
            {
                Response.Clear();
                Response.End();
                return;
            }
            _currLang = _currReservation.cl_pid_lang.HasValue ? _currReservation.cl_pid_lang.Value : 2;
            CurrentLang.ID = _currLang;
            _currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currReservation.pid_estate);
            if (_currEstate == null)
            {
                Response.Clear();
                Response.End();
                return;
            }

            //RadBarcode1.Text = App.HOST_SSL + "/reservationarea/login.aspx?auth=" + _currReservation.unique_id;
            RadBarcode1.Text = App.HOST + "/pdfgenerator/pdf_rnt_reservation_voucherfo.aspx?uid=" + _currReservation.unique_id + _currReservation.uid_2;
            
            decimal payedPartPayment = 0; // somma pagata
            decimal payedTotalPayment = 0; // somma pagata
            if (IdReservation != 0)
            {
                magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
                List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == IdReservation && x.is_complete == 1 && x.direction == 1).ToList();
                foreach (INV_TBL_PAYMENT _currPay in _listPay)
                {
                    payedTotalPayment += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
                    if (_currPay.rnt_reservation_part.StartsWith("part"))
                        payedPartPayment += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
                }
            }
            if (agentTbl != null && agentTbl.IsCustomerPaysAgency == 1)
                payedTotalPayment = _currReservation.pr_total.objToDecimal();

            decimal PayedTotalPaymentDifference = payedTotalPayment - _currReservation.pr_total.objToDecimal();
            decimal PayedPartPaymentDifference = payedPartPayment - _currReservation.pr_part_payment_total.objToDecimal();
            if (PayedTotalPaymentDifference > PayedPartPaymentDifference)
                PayedPartPaymentDifference = PayedTotalPaymentDifference;
            if (payedTotalPayment > _currReservation.pr_total)
                payedTotalPayment = _currReservation.pr_total.objToDecimal();

            // come dormono
            string comeDormono = "";
            if (_currReservation.bedSingle.objToInt32() > 0)
                comeDormono += "<tr><th>" + CurrentSource.getSysLangValue("lblBedSingle") + ":</th><td> " + _currReservation.bedSingle.objToInt32() + "</td></tr>";
            if (_currReservation.bedDouble.objToInt32() > 0)
                comeDormono += "<tr><th>" + CurrentSource.getSysLangValue("lblBedDouble") + ":</th><td> " + _currReservation.bedDouble.objToInt32() + "</td></tr>";
            if (_currReservation.bedDoubleD.objToInt32() > 0)
                comeDormono += "<tr><th>" + CurrentSource.getSysLangValue("lblBedDoubleDivisible") + ":</th><td> " + _currReservation.bedDoubleD.objToInt32() + (_currReservation.bedDoubleDConfig.objToInt32() > 0 ? " (" + _currReservation.bedDoubleDConfig.objToInt32() + " di cui da dividere)" : "") + "</td></tr>";
            if (_currReservation.bedDouble2level.objToInt32() > 0)
                comeDormono += "<tr><th>" + CurrentSource.getSysLangValue("Letti a castello") + ":</th><td> " + _currReservation.bedDouble2level.objToInt32() + "</td></tr>";
            if (_currReservation.bedSofaSingle.objToInt32() > 0)
                comeDormono += "<tr><th>" + CurrentSource.getSysLangValue("lblSofaBedSingle") + "Poltrona Letto:</th><td> " + _currReservation.bedSofaSingle.objToInt32() + "</td></tr>";
            if (_currReservation.bedSofaDouble.objToInt32() > 0)
                comeDormono += "<tr><th>" + CurrentSource.getSysLangValue("lblSofaBedDouble") + "Divano Letto:</th><td> " + _currReservation.bedSofaDouble.objToInt32() + "</td></tr>";
            ltr_comeDormono.Text = comeDormono;


            // terms and conditions
            int termsAndConditionsID = _currEstate.pid_city == 3 ? 34 : 19;
            CONT_VIEW_STP _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == termsAndConditionsID && item.pid_lang == CurrentLang.ID);
            if (_stp == null)
                _stp = maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == termsAndConditionsID && item.pid_lang == 2);
            if (_stp != null)
                ltr_terms.Text = _stp.description;
           // assistenza
            if (_currEstate.is_srs == 1)
            {
                //  Yefrey Guerrero Paredes;welcoming-4@areas.rentalinrome.com;1,+39 329 789 6325
                if (_currEstate.pid_zone == 1)
                    ltr_srs.Text += "+39 329 789 6325,";
                //Vladimir Trbovic;welcoming-5@areas.rentalinrome.com;5;6;17, +39 327 173 2624
                if (_currEstate.pid_zone == 5 || _currEstate.pid_zone == 6 || _currEstate.pid_zone == 17)
                    ltr_srs.Text += "+39 327 173 2624,";
                //Jonny;welcoming-3@areas.rentalinrome.com;4;18;19, +39 327 173 2623
                if (_currEstate.pid_zone == 4 || _currEstate.pid_zone == 9 || _currEstate.pid_zone == 18 || _currEstate.pid_zone == 19)
                    ltr_srs.Text += "+39 327 173 2623,";
                //Filipe Marques;welcoming-1@areas.rentalinrome.com;2;16, +39 327 173 2626
                if (_currEstate.pid_zone == 2 || _currEstate.pid_zone == 16)
                    ltr_srs.Text += "+39 327 173 2626,";
                //Alessandro Pancaldi;welcoming-2@areas.rentalinrome.com;3, +39 327 173 2622
                if (_currEstate.pid_zone == 3)
                    ltr_srs.Text += "+39 327 173 2622,";
                //Alex;reception@rentalinrome.com;19
                if (_currEstate.pid_zone == 19)
                    ltr_srs.Text += ",";
            }
            else
            {

                if (_currEstate.srs_ext_phone != null && _currEstate.srs_ext_phone != "")
                    ltr_srs.Text += _currEstate.srs_ext_phone + ", ";
                if (_currEstate.srs_ext_phone_2 != null && _currEstate.srs_ext_phone_2 != "")
                    ltr_srs.Text += _currEstate.srs_ext_phone_2 + ", ";
                if (_currEstate.srs_ext_phone_3 != null && _currEstate.srs_ext_phone_3 != "")
                    ltr_srs.Text += _currEstate.srs_ext_phone_3 + ", ";
                if (_currEstate.srs_ext_phone_4 != null && _currEstate.srs_ext_phone_4 != "")
                    ltr_srs.Text += _currEstate.srs_ext_phone_4 + ", ";
            }
            IdEstate = _currEstate.id;

            // deposito cauzionale
            pnl_deposit.Visible = tblReservation.pr_deposit.objToDecimal() > 0;

            // tassa di soggiorno
            pnl_overnight_tax.Visible = _currEstate.pr_has_overnight_tax == 1;

            // richiesta del visto
            if (_currReservation.visa_isRequested == 1)
            {
                List<RNT_RL_RESERVATION_PERSON> _visaList = DC_RENTAL.RNT_RL_RESERVATION_PERSON.Where(x => x.pid_reservation == _currReservation.id).ToList();
                if (_visaList.Count > 0)
                { 
                    pnl_visa_isRequested.Visible = true;
                    LV_visa.DataSource = _visaList;
                    LV_visa.DataBind();
                }
            }

            HF_numPers.Value = "" + (_currReservation.num_adult.objToInt32() + _currReservation.num_child_min.objToInt32() + _currReservation.num_child_over.objToInt32());
            DateTime dtStart = _currReservation.dtStart.Value;
            DateTime dtEnd = _currReservation.dtEnd.Value;

            int _nights = (dtEnd - dtStart).Days;
            HF_nightsTotal.Value = _nights.ToString();
            int _weeks = 0;
            while(_nights>=7)
            {
                _weeks++;
                _nights -= 7;
            }
            HF_weeks.Value = _weeks.ToString();
            HF_nights.Value = _nights.ToString();
            //txt_birth_place.Text = _client.birth_place;
            //HF_birth_date.Value = _client.birth_date.HasValue ? _client.birth_date.JSCal_dateToString() : "19800101";
            //Bind_drp_doc_type();
            //drp_doc_type.setSelectedValue(_client.doc_type);
            //txt_doc_num.Text = _client.doc_num;
            //txt_doc_issue_place.Text = _client.doc_issue_place;
            //HF_doc_expiry_date.Value = _client.doc_expiry_date.HasValue ? _client.doc_expiry_date.JSCal_dateToString() : "20150101";
            //txt_loc_address.Text = _client.loc_address;
            //txt_loc_state.Text = _client.loc_state;
            //txt_loc_zip_code.Text = _client.loc_zip_code;
            //txt_loc_city.Text = _client.loc_city;
            //txt_contact_phone_mobile.Text = _client.contact_phone_mobile;
            //txt_contact_phone_trip.Text = _client.contact_phone_trip;
            //txt_contact_phone.Text = _client.contact_phone;
            //txt_contact_fax.Text = _client.contact_fax;
            //txt_contact_email.Text = _currTBL.cl_email;

        }
        protected USR_TBL_CLIENT getClientFromAgent(long id)
        {
            USR_TBL_CLIENT tmpClient = new USR_TBL_CLIENT();
            var agClient = authProps.ClientTBL.SingleOrDefault(x => x.id == id);
            if(agClient==null)
                return tmpClient;

            tmpClient.pid_lang = agClient.pidLang.ToInt32();
            tmpClient.name_full = agClient.nameFull;
            tmpClient.name_honorific = agClient.nameHonorific;
            tmpClient.birth_place = agClient.birthPlace;
            tmpClient.birth_date = agClient.birthDate;
            tmpClient.doc_type = agClient.docType;
            tmpClient.doc_cf_num = agClient.docCf;
            tmpClient.doc_vat_num = agClient.docVat;
            tmpClient.doc_num = agClient.docNum;
            tmpClient.doc_issue_place = agClient.docIssuePlace;
            tmpClient.doc_issue_date = agClient.docIssueDate;
            tmpClient.doc_expiry_date = agClient.docExpiryDate;
            tmpClient.loc_address = agClient.locAddress;
            tmpClient.loc_state = agClient.locState;
            tmpClient.loc_zip_code = agClient.locZipCode;
            tmpClient.loc_city = agClient.locCity;
            tmpClient.contact_phone_mobile = agClient.contactPhoneMobile;
            tmpClient.contact_phone_trip = agClient.contactPhoneTrip;
            tmpClient.contact_phone = agClient.contactPhone;
            tmpClient.contact_fax = agClient.contactFax;
            tmpClient.contact_email = agClient.contactEmail;
            tmpClient.loc_country = agClient.locCountry;
            return tmpClient;
        }
    }
}
