using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;
using System.IO;
using ModAuth;

namespace RentalInRome.admin
{
    public partial class rnt_reservation_details_manuale : adminBasePage
    {
        protected string listPage = "rnt_reservation_list.aspx";
        private magaRental_DataContext DC_RENTAL;
        protected RNT_TBL_RESERVATION currTbl;
        private magaInvoice_DataContext DC_INVOICE;
        public RNT_TBL_RESERVATION tblReservation
        {
            get
            {
                if (currTbl == null)
                {
                    DC_RENTAL = maga_DataContext.DC_RENTAL;
                    currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                    if (currTbl == null)
                    {
                        Response.Redirect(listPage, true);
                        Response.End();
                    }
                }
                return currTbl ?? new RNT_TBL_RESERVATION();
            }
        }
        public long IdReservation
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        public long IdRequest
        {
            get { return HF_IdRequest.Value.ToInt64(); }
            set { HF_IdRequest.Value = value.ToString(); }
        }
        public int dtCount
        {
            get
            {
                return HF_dtCount.Value.ToInt32();
            }
            set
            {
                HF_dtCount.Value = value.ToString();
            }
        }
        private rntExts.PreReservationPrices TMPcurrOutPrice;
        public rntExts.PreReservationPrices currOutPrice
        {
            get
            {
                if (TMPcurrOutPrice == null)
                    TMPcurrOutPrice = (rntExts.PreReservationPrices)ViewState["_currOutPrice"];
                return TMPcurrOutPrice ?? new rntExts.PreReservationPrices();
            }
            set { TMPcurrOutPrice = value; ViewState["_currOutPrice"] = TMPcurrOutPrice; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_reservation_list";
            UC_estate.onModify += new EventHandler(UC_estate_onModify);
            UC_estate.onSave += new EventHandler(UC_estate_onSave);
            UC_estate.onCancel += new EventHandler(UC_estate_onCancel);

            UC_state.onModify += new EventHandler(UC_state_onModify);
            UC_state.onSave += new EventHandler(UC_state_onSave);
            UC_state.onCancel += new EventHandler(UC_state_onCancel);

            UC_client.onModify += new EventHandler(UC_client_onModify);
            UC_client.onSave += new EventHandler(UC_client_onSave);
            UC_client.onCancel += new EventHandler(UC_client_onCancel);

            UC_dt_pers.onModify += new EventHandler(UC_dt_pers_onModify);
            UC_dt_pers.onSave += new EventHandler(UC_dt_pers_onSave);
            UC_dt_pers.onCancel += new EventHandler(UC_dt_pers_onCancel);

            UC_block.onModify += new EventHandler(UC_block_onModify);
            UC_block.onSave += new EventHandler(UC_block_onSave);
            UC_block.onCancel += new EventHandler(UC_block_onCancel);

            UC_notesInner.onModify += new EventHandler(UC_notesInner_onModify);
            UC_notesInner.onSave += new EventHandler(UC_notesInner_onSave);
            UC_notesInner.onCancel += new EventHandler(UC_notesInner_onCancel);

            UC_notesEco.onModify += new EventHandler(UC_notesEco_onModify);
            UC_notesEco.onSave += new EventHandler(UC_notesEco_onSave);
            UC_notesEco.onCancel += new EventHandler(UC_notesEco_onCancel);

            UC_notesClient.onModify += new EventHandler(UC_notesClient_onModify);
            UC_notesClient.onSave += new EventHandler(UC_notesClient_onSave);
            UC_notesClient.onCancel += new EventHandler(UC_notesClient_onCancel);


            UC_inout.onModify += new EventHandler(UC_inout_onModify);
            UC_inout.onSave += new EventHandler(UC_inout_onSave);
            UC_inout.onCancel += new EventHandler(UC_inout_onCancel);

            ucProblemSelect.onModify += new EventHandler(ucProblemSelect_onModify);
            ucProblemSelect.onSave += new EventHandler(ucProblemSelect_onSave);
            ucProblemSelect.onCancel += new EventHandler(ucProblemSelect_onCancel);

            ucAgent.onModify += new EventHandler(ucAgent_onModify);
            ucAgent.onSave += new EventHandler(ucAgent_onSave);
            ucAgent.onCancel += new EventHandler(ucAgent_onCancel);

            ucAgentClient.onModify += new EventHandler(ucAgentClient_onModify);
            ucAgentClient.onSave += new EventHandler(ucAgentClient_onSave);
            ucAgentClient.onCancel += new EventHandler(ucAgentClient_onCancel);

            ucPayment.onSave += new EventHandler(ucPayment_onSave);

            ucResRefund.onSave += new EventHandler(ucResRefund_onSave);
        }
        void ucPayment_onSave(object sender, EventArgs e)
        {
            if (ucPayment.Reload)
                Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation, true);
        }
        void ucResRefund_onSave(object sender, EventArgs e)
        {
            if (ucResRefund.Reload)
                Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation, true);
        }
        void UC_estate_onModify(object sender, EventArgs e)
        {
            UC_estate.IsEdit = true;
            lockAll(true);
            UC_estate.IsLocked = false;
        }
        void UC_estate_onSave(object sender, EventArgs e)
        {
            UC_estate.IsEdit = false;
            UC_dt_pers._checkCalDates = rntUtils.rntEstate_availableDatesForJSCal(UC_estate.IdEstate, IdReservation);
            UC_dt_pers.num_persons_child = UC_estate.num_persons_child;
            UC_dt_pers.num_persons_min = UC_estate.num_persons_min;
            UC_dt_pers.num_persons_max = UC_estate.num_persons_max;
            lockAll(false);
            saveReservation("estate");
        }
        void UC_estate_onCancel(object sender, EventArgs e)
        {
            UC_estate.IsEdit = false;
            lockAll(false);
        }
        void UC_state_onModify(object sender, EventArgs e)
        {
            UC_state.IsEdit = true;
            lockAll(true);
            UC_state.IsLocked = false;
        }
        void UC_state_onSave(object sender, EventArgs e)
        {
            HF_is_booking.Value = UC_state.state_pid == 1 || UC_state.state_pid == 2 ? "0" : "1";
            UC_state.IsEdit = false;
            lockAll(false);
            saveReservation("state");
        }
        void UC_state_onCancel(object sender, EventArgs e)
        {
            UC_state.IsEdit = false;
            lockAll(false);
        }
        void UC_client_onModify(object sender, EventArgs e)
        {
            lockAll(true);
            UC_client.IsLocked = false;
            UC_client.IsEdit = true;
        }
        void UC_client_onSave(object sender, EventArgs e)
        {
            UC_client.IsEdit = false;
            lockAll(false);
            saveReservation("client");
        }
        void UC_client_onCancel(object sender, EventArgs e)
        {
            UC_client.IsEdit = false;
            lockAll(false);
        }
        void UC_dt_pers_onModify(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = true;
            lockAll(true);
            UC_dt_pers.IsLocked = false;
        }
        void UC_dt_pers_onSave(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = false;
            UC_estate.sel_dtStart = UC_dt_pers.sel_dtStart;
            UC_estate.sel_dtEnd = UC_dt_pers.sel_dtEnd;
            UC_estate.sel_num_persons = UC_dt_pers.sel_num_adult + UC_dt_pers.sel_num_child_over;
            lockAll(false);
            saveReservation("dt_pers");
        }
        void UC_dt_pers_onCancel(object sender, EventArgs e)
        {
            UC_dt_pers.IsEdit = false;
            lockAll(false);
        }
        void UC_block_onModify(object sender, EventArgs e)
        {
            UC_block.IsEdit = true;
            lockAll(true);
            UC_block.IsLocked = false;
        }
        void UC_block_onSave(object sender, EventArgs e)
        {
            saveReservation("block_expire");
            UC_block.IsEdit = false;
            lockAll(false);
        }
        void UC_block_onCancel(object sender, EventArgs e)
        {
            UC_block.IsEdit = false;
            lockAll(false);
        }
        void UC_notesInner_onModify(object sender, EventArgs e)
        {
            UC_notesInner.IsEdit = true;
            lockAll(true);
            UC_notesInner.IsLocked = false;
        }
        void UC_notesInner_onSave(object sender, EventArgs e)
        {
            saveReservation("notesInner");
            UC_notesInner.IsEdit = false;
            lockAll(false);
        }
        void UC_notesInner_onCancel(object sender, EventArgs e)
        {
            UC_notesInner.IsEdit = false;
            lockAll(false);
        }
        void UC_notesEco_onModify(object sender, EventArgs e)
        {
            UC_notesEco.IsEdit = true;
            lockAll(true);
            UC_notesEco.IsLocked = false;
        }
        void UC_notesEco_onSave(object sender, EventArgs e)
        {
            saveReservation("notesEco");
            UC_notesEco.IsEdit = false;
            lockAll(false);
        }
        void UC_notesEco_onCancel(object sender, EventArgs e)
        {
            UC_notesEco.IsEdit = false;
            lockAll(false);
        }

        void UC_notesClient_onModify(object sender, EventArgs e)
        {
            UC_notesClient.IsEdit = true;
            lockAll(true);
            UC_notesClient.IsLocked = false;
        }
        void UC_notesClient_onSave(object sender, EventArgs e)
        {
            saveReservation("notesClient");
            UC_notesClient.IsEdit = false;
            lockAll(false);
        }
        void UC_notesClient_onCancel(object sender, EventArgs e)
        {
            UC_notesClient.IsEdit = false;
            lockAll(false);
        }
        void UC_inout_onModify(object sender, EventArgs e)
        {
            UC_inout.IsEdit = true;
            lockAll(true);
            UC_inout.IsLocked = false;
        }
        void UC_inout_onSave(object sender, EventArgs e)
        {
            UC_inout.IsEdit = false;
            lockAll(false);
            saveReservation("inout");
        }
        void UC_inout_onCancel(object sender, EventArgs e)
        {
            UC_inout.IsEdit = false;
            lockAll(false);
        }
        void ucProblemSelect_onModify(object sender, EventArgs e)
        {
            ucProblemSelect.IsEdit = true;
            lockAll(true);
            ucProblemSelect.IsLocked = false;
        }
        void ucProblemSelect_onSave(object sender, EventArgs e)
        {
            ucProblemSelect.IsEdit = false;
            lockAll(false);
            saveReservation("problem");
        }
        void ucProblemSelect_onCancel(object sender, EventArgs e)
        {
            ucProblemSelect.IsEdit = false;
            lockAll(false);
        }
        void ucAgent_onModify(object sender, EventArgs e)
        {
            ucAgent.IsEdit = true;
            lockAll(true);
            ucAgent.IsLocked = false;
        }
        void ucAgent_onSave(object sender, EventArgs e)
        {
            ucAgent.IsEdit = false;
            lockAll(false);
            saveReservation("agent");
        }
        void ucAgent_onCancel(object sender, EventArgs e)
        {
            ucAgent.IsEdit = false;
            lockAll(false);
        }

        void ucAgentClient_onModify(object sender, EventArgs e)
        {
            ucAgentClient.IsEdit = true;
            lockAll(true);
            ucAgentClient.IsLocked = false;
        }
        void ucAgentClient_onSave(object sender, EventArgs e)
        {
            ucAgentClient.IsEdit = false;
            lockAll(false);
            saveReservation("agentClient");
        }
        void ucAgentClient_onCancel(object sender, EventArgs e)
        {
            ucAgentClient.IsEdit = false;
            lockAll(false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            string str = AppDomain.CurrentDomain.BaseDirectory;
            if (!IsPostBack)
            {
                IdReservation = Request.QueryString["id"].ToInt64();
                currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
                if (currTbl == null)
                {
                    Response.Redirect(listPage, true);
                    return;
                }
                if (currTbl.id < 150000)
                {
                    Response.Redirect(listPage, true);
                    return;
                }
                if (currTbl.pr_part_modified != 1)
                {
                    Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation, true);
                    return;
                }
                HF_uid.Value = currTbl.unique_id.ToString();
                pnl_mailConfirm.Visible = currTbl.state_pid == 4;
                pnl_mailCreation.Visible = currTbl.state_pid == 6;

                string url_voucher = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + currTbl.uid_2;
                string url_voucher_pdf = CurrentAppSettings.HOST_SSL + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_voucher.aspx?uid=" + currTbl.uid_2;
                string filename = "reservation_voucher-code_" + currTbl.code + ".pdf";
                
                HL_getPdf.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher_pdf.urlEncode() + "&filename=" + filename.urlEncode();
                HL_viewPdf.NavigateUrl = url_voucher;

                HL_reservationarea.NavigateUrl = CurrentAppSettings.ROOT_PATH + "reservationarea/login.aspx?auth=" + currTbl.unique_id;
                pnl_reservationarea.Visible = (UserAuthentication.CURRENT_USER_ROLE == 1 || UserAuthentication.CURRENT_USER_ROLE == 3 || UserAuthentication.CURRENT_USER_ROLE == 9);

                HL_ccData.NavigateUrl = CurrentAppSettings.ROOT_PATH + "admin/dati_cc/" + currTbl.unique_id + ".txt";
                //for making credit card panel visible to some specific user
                var allowd_cc_users = CommonUtilities.getSYS_SETTING("cc_view_users").splitStringToList(",").Select(x => x).ToList();
                PH_ccData.Visible = (UserAuthentication.CURRENT_USER_ROLE == 1 || allowd_cc_users.Contains(UserAuthentication.CurrentUserID + "")) && currTbl.pr_paymentType == "cc";
                //PH_ccData.Visible = (UserAuthentication.CURRENT_USER_ROLE == 1) && currTbl.pr_paymentType == "cc";

                try
                {
                    if (File.Exists(Server.MapPath("~/admin/dati_cc/" + currTbl.unique_id + ".txt")))
                    {
                        //txtCC1.Text = File.ReadAllText(Server.MapPath("~/admin/dati_cc/" + currTbl.unique_id + ".txt"));

                        using (FileStream stream = File.Open(Server.MapPath("~/admin/dati_cc/" + currTbl.unique_id + ".txt"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                try
                                {
                                    txtCC1.Text += reader.ReadToEnd();
                                }
                                catch (Exception ex)
                                {
                                    ErrorLog.addLog("", "ResDett_complete.fillData.CCDataRead IdReservation: " + IdReservation, ex.ToString());
                                }
                            }
                        }

                        //if(!string.IsNullOrEmpty(txtCC1.Text))
                        txtCC2.Text = RijndaelSimple_2.Decrypt(txtCC1.Text);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.addLog("", "ResDett_complete.fillData.CCData IdReservation: " + IdReservation, ex.ToString());
                }
                HF_code.Value = currTbl.code;

                UC_block.block_pid_user = currTbl.block_pid_user.objToInt32();
                UC_block.block_expire_hours = currTbl.block_expire_hours.objToInt32();
                UC_block.block_comments = currTbl.block_comments;
                UC_block.dttCreation = currTbl.dtCreation.Value;
                UC_block.FillControls();

                UC_inout.IdReservation = IdReservation;
                UC_inout.UIdReservation = currTbl.unique_id.ToString();
                UC_inout.in_pointType = currTbl.limo_inPoint_type;
                UC_inout.in_transportType = currTbl.limo_inPoint_transportType;
                UC_inout.in_pickupPlace = currTbl.limo_inPoint_pickupPlace.objToInt32();
                UC_inout.in_pickupPlaceName = currTbl.limo_inPoint_pickupPlaceName;
                UC_inout.in_pickupDetails = currTbl.limo_inPoint_details;
                UC_inout.in_pickupDetailsType = currTbl.limo_inPoint_detailsType.objToInt32();
                UC_inout.limo_in_datetime = currTbl.limo_in_datetime;
                if (!currTbl.dtIn.HasValue && !string.IsNullOrEmpty(currTbl.dtStartTime) && currTbl.dtStartTime != "000000")
                    UC_inout.dtIn = currTbl.dtStart.Value.Add(currTbl.dtStartTime.JSTime_stringToTime());
                else
                    UC_inout.dtIn = currTbl.dtIn;
                UC_inout.out_pointType = currTbl.limo_outPoint_type;
                UC_inout.out_transportType = currTbl.limo_outPoint_transportType;
                UC_inout.out_pickupPlace = currTbl.limo_outPoint_pickupPlace.objToInt32();
                UC_inout.out_pickupPlaceName = currTbl.limo_outPoint_pickupPlaceName;
                UC_inout.out_pickupDetails = currTbl.limo_outPoint_details;
                UC_inout.out_pickupDetailsType = currTbl.limo_outPoint_detailsType.objToInt32();
                UC_inout.limo_out_datetime = currTbl.limo_out_datetime;
                if (!currTbl.dtOut.HasValue && !string.IsNullOrEmpty(currTbl.dtEndTime) && currTbl.dtEndTime != "000000")
                    UC_inout.dtOut = currTbl.dtEnd.Value.Add(currTbl.dtEndTime.JSTime_stringToTime());
                else
                    UC_inout.dtOut = currTbl.dtOut;
                UC_inout.FillControls();

                rntUtils.rntReservation_setDefaults(ref currTbl);
                DC_RENTAL.SubmitChanges();
                UC_estate.IdEstate = currTbl.pid_estate.objToInt32();
                UC_estate.IdReservation = IdReservation;
                UC_estate.sel_dtStart = currTbl.dtStart.Value;
                UC_estate.sel_dtEnd = currTbl.dtEnd.Value;
                UC_estate.sel_num_persons = currTbl.num_adult.objToInt32() + currTbl.num_child_over.objToInt32();
                UC_estate.sel_pr_total = currTbl.pr_total.objToDecimal();
                UC_estate.FillControls();

                UC_state.IdReservation = IdReservation;
                UC_state.FillControls();
                pnl_requestRenewal.Visible = currTbl.requestRenewal.objToInt32() < 1 && currTbl.state_pid == 3 && currTbl.dtCreation.Value.AddHours(96) > DateTime.Now;
                lbl_requestRenewal_NO.Visible = currTbl.requestRenewal.objToInt32() < 1 && currTbl.state_pid == 3 && currTbl.dtCreation.Value.AddHours(96) < DateTime.Now;
                lbl_requestRenewal.Visible = currTbl.requestRenewal.objToInt32() > 0 && currTbl.state_pid == 3;
                lbl_requestRenewal_OLD.Visible = currTbl.requestRenewal.objToInt32() == -1;
                pnl_requestRenewalCont.Visible = pnl_requestRenewal.Visible || lbl_requestRenewal.Visible || lbl_requestRenewal_NO.Visible || lbl_requestRenewal_OLD.Visible;

                UC_notesInner.Body = currTbl.notesInner;
                UC_notesInner.FillControls();

                ucProblemSelect.ProblemID = currTbl.problemID.objToInt32();
                ucProblemSelect.ProblemDesc = currTbl.problemDesc;
                ucProblemSelect.FillControls();

                //display creditcard data of extraservice only to superadmin
                //HL_ccData.NavigateUrl = CurrentAppSettings.ROOT_PATH + "admin/dati_cc/" + _currTBL.unique_id + "_extraservices.txt";
                //PH_ccData.Visible = (UserAuthentication.CURRENT_USER_ROLE == 1);
                div_creditCard.Visible = (UserAuthentication.CURRENT_USER_ROLE == 1);
                string line;
                if (div_creditCard.Visible == true)
                {
                    string filePath = App.SRP + "admin/dati_cc/" + currTbl.unique_id + "_extraservices.txt";
                    if (File.Exists(filePath))
                    {
                        StreamReader ccFile = new StreamReader(App.SRP + "admin/dati_cc/" + currTbl.unique_id + "_extraservices.txt");
                        //string myString = RijndaelSimple_2.Decrypt(ccFile.ReadToEnd());
                        while ((line = ccFile.ReadLine()) != null)
                        {
                            if (lbl_cc.Text == "")
                            {
                                lbl_cc.Text += RijndaelSimple_2.Decrypt(line);
                            }
                            else
                            {
                                lbl_cc.Text += "</br>" + RijndaelSimple_2.Decrypt(line);
                            }

                        }

                    }

                }


                // come dormono
                string comeDormono = "";
                if (currTbl.bedSingle.objToInt32() > 0)
                    comeDormono += "<tr><th>Letti single:</th><td> " + currTbl.bedSingle.objToInt32() + "</td></tr>";
                if (currTbl.bedDouble.objToInt32() > 0)
                    comeDormono += "<tr><th>Matrimoniali:</th><td> " + currTbl.bedDouble.objToInt32() + "</td></tr>";
                if (currTbl.bedDoubleD.objToInt32() > 0)
                    comeDormono += "<tr><th>Matrimoniali divisibili:</th><td> " + currTbl.bedDoubleD.objToInt32() + (currTbl.bedDoubleDConfig.objToInt32() > 0 ? " (" + currTbl.bedDoubleDConfig.objToInt32() + " di cui da dividere)" : "") + "</td></tr>";
                if (currTbl.bedDouble2level.objToInt32() > 0)
                    comeDormono += "<tr><th>Letti a castello:</th><td> " + currTbl.bedDouble2level.objToInt32() + "</td></tr>";
                if (currTbl.bedSofaSingle.objToInt32() > 0)
                    comeDormono += "<tr><th>Poltrona Letto:</th><td> " + currTbl.bedSofaSingle.objToInt32() + "</td></tr>";
                if (currTbl.bedSofaDouble.objToInt32() > 0)
                    comeDormono += "<tr><th>Divano Letto:</th><td> " + currTbl.bedSofaDouble.objToInt32() + "</td></tr>";
                if (comeDormono == "")
                    comeDormono = "Non specificato.";
                else
                    comeDormono = "<table>" + comeDormono + "</table>";
                ltr_comeDormono.Text = comeDormono;

                UC_notesEco.Body = currTbl.notesEco;
                UC_notesEco.FillControls();

                UC_notesClient.Body = currTbl.notesClient;
                UC_notesClient.FillControls();


                UC_connectionlog.IdReservation = IdReservation;
                UC_connectionlog.FillControls();


                // todo Agetnzie da finire
                ucAgent.IdAgent = currTbl.agentID.objToInt64();
                ucAgent.IdReservation = currTbl.id;
                ucAgent.FillControls();

                ucAgentClient.IdReservation = IdReservation;
                ucAgentClient.IdAgent = ucAgent.IdAgent;
                ucAgentClient.IdClient = currTbl.agentClientID.objToInt64();
                if (ucAgent.IdAgent == 0)
                    ucAgentClient.Visible = false;
                else
                    ucAgentClient.FillControls();

                UC_client.IdReservation = IdReservation;
                UC_client.IdClient = currTbl.cl_id.objToInt32();
                if (ucAgent.IdAgent != 0)
                    UC_client.Visible = false;
                else
                    UC_client.FillControls();

                UC_dt_pers._checkCalDates = rntUtils.rntEstate_availableDatesForJSCal(UC_estate.IdEstate, IdReservation);
                UC_dt_pers.num_persons_child = UC_estate.num_persons_child;
                UC_dt_pers.num_persons_min = UC_estate.num_persons_min;
                UC_dt_pers.num_persons_max = UC_estate.num_persons_max;
                UC_dt_pers.nights_min = UC_estate.nights_min;
                UC_dt_pers.sel_num_adult = currTbl.num_adult.objToInt32();
                UC_dt_pers.sel_num_child_min = currTbl.num_child_min.objToInt32();
                UC_dt_pers.sel_num_child_over = currTbl.num_child_over.objToInt32();
                UC_dt_pers.sel_dtStart = currTbl.dtStart.Value;
                UC_dt_pers.sel_dtEnd = currTbl.dtEnd.Value;
                UC_dt_pers.FillControls();
                if (currTbl.dtEnd.Value < DateTime.Now.Date.AddDays(-7))
                {
                    UC_client.IsLocked = true;
                    UC_dt_pers.IsLocked = true;
                    UC_estate.IsLocked = true;
                }

                pricesShow(false);
                ucPayment.IdReservation = IdReservation;
                ucResRefund.IdReservation = IdReservation;

                fillExtraServices(currTbl);
                fillExtraRequests(currTbl);

                pnl_notifyNew.Visible = false;
                HF_periodOK.Value = "1";
                HF_avvOK.Value = "1";
                HF_priceOK.Value = "1";
                HF_saveOK.Value = "1";
                rntUtils.reservation_checkPartPayment(currTbl, true);
                checkSave();
            }
        }
        protected string getStateClass(int? id)
        {
            if (id == 1)
                return "rntCal_nd";
            if (id == 2)
                return "rntCal_opz";
            if (id == 3)
                return "rntCal_can";
            if (id == 4)
                return "rntCal_prt";
            if (id == 5)
                return "rntCal_mv";
            return "rntCal_xxx";
        }

        protected void lockAll(bool _lock)
        {
            UC_client.IsLocked = _lock;
            UC_dt_pers.IsLocked = _lock;
            UC_estate.IsLocked = _lock;
            UC_state.IsLocked = _lock;
            UC_block.IsLocked = _lock;
            UC_notesInner.IsLocked = _lock;
            if (UC_dt_pers.sel_dtStart <= DateTime.Now.Date.AddDays(-7))
            {
                UC_client.IsLocked = true;
                UC_dt_pers.IsLocked = true;
                UC_estate.IsLocked = true;
            }
        }
        protected void fillExtraServices(RNT_TBL_RESERVATION currTBL)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntReservationExtrasTMP> currRes = dc.dbRntReservationExtrasTMPs.Where(x => x.pidReservation == currTBL.id).ToList();
                List<dbRntExtraOwnerTBL> lstOwner = new List<dbRntExtraOwnerTBL>();
                if (currRes != null && currRes.Count > 0)
                {
                    LV_extraServices.DataSource = currRes;
                    LV_extraServices.DataBind();

                }
                else
                {
                    div_service.Visible = false;
                }
                foreach (dbRntReservationExtrasTMP res in currRes)
                {
                    dbRntEstateExtrasTB currExtra = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == res.pidExtras);
                    if (currExtra != null)
                    {
                        if (currExtra.pidOwner != null)
                        {
                            dbRntExtraOwnerTBL currOwner = dc.dbRntExtraOwnerTBLs.SingleOrDefault(x => x.id == currExtra.pidOwner);
                            if (currOwner != null)
                            {
                                if (!lstOwner.Contains(currOwner))
                                    lstOwner.Add(currOwner);
                            }
                        }
                    }
                }
                if (lstOwner != null && lstOwner.Count > 0)
                {
                    div_extraOwner.Visible = true;
                    foreach (dbRntExtraOwnerTBL objOwner in lstOwner)
                    {
                        if (!string.IsNullOrEmpty(objOwner.nameCompany))
                        {
                            chk_extrasercices.Items.Add(new ListItem(objOwner.nameCompany, Convert.ToString(objOwner.id)));
                        }
                    }

                    //chk_extrasercices.DataSource = lstOwner;
                    //chk_extrasercices.DataTextField = "nameCompany";
                    //chk_extrasercices.DataValueField = "id";
                    //chk_extrasercices.DataBind();


                }
                else
                {
                    div_extraOwner.Visible = false;
                }
                //else
                //{
                //    div_service.Visible = false;
                //}
            }
        }

        protected void fillExtraRequests(RNT_TBL_RESERVATION currTBL)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntExtrasRequest> currReq = dc.dbRntExtrasRequests.Where(x => x.pidReservation == currTBL.id).ToList();
                if (currReq != null && currReq.Count > 0)
                {
                    LV_requests.DataSource = currReq;
                    LV_requests.DataBind();

                }
                else
                {
                    div_request.Visible = false;
                }


            }
        }

        protected void LV_extraServices_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_name = (Label)e.Item.FindControl("lbl_name");
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            Label lbl_date = (Label)e.Item.FindControl("lbl_date");

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    dbRntReservationExtrasTMP currRes = (dbRntReservationExtrasTMP)e.Item.DataItem;
                    if (currRes != null)
                    {
                        DateTime dt = currRes.inDate.Value;
                        lbl_date.Text = dt.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "");
                        dbRntEstateExtrasLN objEstateExtrasLN = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == currRes.pidExtras && x.pidLang == App.LangID);
                        if (objEstateExtrasLN != null)
                        {
                            lbl_name.Text = objEstateExtrasLN.title;

                        }

                    }
                }
            }
        }

        protected void LV_requests_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_name = (Label)e.Item.FindControl("lbl_name");
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            Label lbl_date = (Label)e.Item.FindControl("lbl_date");

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    dbRntExtrasRequest currRes = (dbRntExtrasRequest)e.Item.DataItem;
                    if (currRes != null)
                    {
                        DateTime dt = currRes.createdDate.Value;
                        lbl_date.Text = dt.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "");
                        dbRntEstateExtrasLN objEstateExtrasLN = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == currRes.pidExtra && x.pidLang == App.LangID);
                        if (objEstateExtrasLN != null)
                        {
                            lbl_name.Text = objEstateExtrasLN.title;

                        }

                    }
                }
            }
        }

        protected void checkSave()
        {
            pnl_notifyNew.Visible = IdReservation == 0 && HF_is_booking.Value == "1";
            lbl_avvError.Visible = false;
            lbl_periodError.Visible = false;
            lbl_priceError.Visible = false;
            lbl_clientError.Visible = false;
            lbl_estateError.Visible = false;
            lbl_stateError.Visible = false;
            bool _ok = true;
            if (HF_avvOK.Value == "0")
            {
                lbl_avvError.Visible = true;
                _ok = false;
            }
            if ((HF_periodOK.Value == "0" || UC_dt_pers.IsEdit) && HF_is_booking.Value == "1")
            {
                lbl_periodError.Visible = true;
                _ok = false;
            }
            if (HF_priceOK.Value == "0" && HF_is_booking.Value == "1")
            {
                lbl_priceError.Visible = true;
                _ok = false;
            }
            if ((UC_client.IdClient == 0 || UC_client.IsEdit) && HF_is_booking.Value == "1")
            {
                lbl_clientError.Visible = true;
                _ok = false;
            }
            if (UC_estate.IdEstate == 0 || UC_estate.IsEdit)
            {
                lbl_estateError.Visible = true;
                _ok = false;
            }
            if (UC_state.state_pid == 0 || UC_state.IsEdit)
            {
                lbl_stateError.Visible = true;
                _ok = false;
            }
            if (HF_is_booking.Value != "1")
            {
                UC_dt_pers.IsBooking = false;
                UC_client.Visible = false;
            }
            else
            {
                UC_dt_pers.IsBooking = true;
                //UC_client.Visible = ucAgent.IdAgent == 0;
            }
            pnl_btnSave.Visible = _ok;
            UpdatePanel_main.Update();
        }

        protected void lnk_calculatePrice_Click(object sender, EventArgs e)
        {
            checkAvailability();
        }
        public bool checkAvailability()
        {
            HF_periodOK.Value = "1";
            HF_avvOK.Value = "1";
            HF_priceOK.Value = "0";
            rntExts.PreReservationPrices outPrice = new rntExts.PreReservationPrices();
            RNT_TBL_RESERVATION tmp = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (tmp == null)
            {
                Response.Redirect(listPage, true);
                return false;
            }
            outPrice.FillFrom(tmp);
            outPrice.dtStart = UC_dt_pers.sel_dtStart;
            outPrice.dtEnd = UC_dt_pers.sel_dtEnd;
            outPrice.numPersCount = (UC_dt_pers.sel_num_adult + UC_dt_pers.sel_num_child_over);
            outPrice.pr_discount_owner = tmp.pr_discount_owner.objToDecimal();
            outPrice.pr_discount_commission = tmp.pr_discount_commission.objToDecimal();
            outPrice.part_percentage = UC_estate.pr_percentage;
            if (ucAgent.IdAgent != 0)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var agentTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == ucAgent.IdAgent);
                    if (agentTBL != null)
                    {
                        outPrice.agentID = agentTBL.id;
                        outPrice.agentDiscountType = agentTBL.pidDiscountType.objToInt32();
                        outPrice.agentDiscountNotPayed = agentTBL.payDiscountNotPayed.objToInt32(); ;
                        outPrice.requestFullPayAccepted = agentTBL.payFullPayment.objToInt32();
                        if (outPrice.agentDiscountType == 0) outPrice.agentDiscountType = 1;
                    }
                }
            }
            decimal _pr_total = rntUtils.rntEstate_getPrice(IdReservation, UC_estate.IdEstate, ref outPrice);
            decimal payedPart = 0; // somma pagata
            if (IdReservation != 0)
            {
                magaInvoice_DataContext DC_INVOICE = maga_DataContext.DC_INVOICE;
                List<INV_TBL_PAYMENT> _listPay = DC_INVOICE.INV_TBL_PAYMENT.Where(x => x.rnt_pid_reservation == IdReservation && x.rnt_reservation_part.StartsWith("part") && x.is_complete == 1 && x.direction == 1).ToList();
                foreach (INV_TBL_PAYMENT _currPay in _listPay)
                {
                    payedPart += _currPay.pr_total.objToDecimal(); // somma tutti pagamenti effettuati
                }
            }

            bool _isAvailable = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.Where(y => y.pid_estate == UC_estate.IdEstate && y.id != IdReservation //
                                                                            && y.state_pid != 3 //
                                                                            && y.dtStart.HasValue //
                                                                            && y.dtEnd.HasValue //
                                                                            && ((y.dtStart.Value.Date <= outPrice.dtStart && y.dtEnd.Value.Date >= outPrice.dtEnd) //
                                                                                || (y.dtStart.Value.Date >= outPrice.dtStart && y.dtStart.Value.Date < outPrice.dtEnd) //
                                                                                || (y.dtEnd.Value.Date > outPrice.dtStart && y.dtEnd.Value.Date <= outPrice.dtEnd))).Count() == 0;
            outPrice.prTotal = _pr_total;
            currOutPrice = outPrice;
            lbl_priceError1.Visible = outPrice.outError == 1;
            lbl_priceError2.Visible = outPrice.outError == 2;
            if (!_isAvailable)
            {
                HF_avvOK.Value = "0";
                return false;
            }
            UC_estate.sel_pr_total = _pr_total;
            HF_priceOK.Value = "1";
            return true;
        }
        protected void savePrices(ref RNT_TBL_RESERVATION currTBL)
        {
            currOutPrice.CopyTo(ref currTBL);
        }
        protected void saveReservation(string type)
        {
            bool reservationCancelled = false;
            bool _reload = false;
            bool _checkPayment = false;
            bool _isStartDateTimeChanged = false;
            bool _isEndDateTimeChanged = false;


            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                Response.Redirect(listPage, true);
                return;
            }
            if (type == "estate")
            {
                if (checkAvailability())
                {
                    currTbl.pid_estate = UC_estate.IdEstate;
                    currTbl.pr_deposit = UC_estate.pr_deposit;
                    currTbl.pr_depositWithCard = UC_estate.pr_depositWithCard ? 1 : 0;
                    savePrices(ref currTbl);
                    _checkPayment = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('La struttura selezionata non risulta disponibile nel periodo, \nnessun salvataggio eseguito\nsi prega di aggiornare la pagina!')", true);
                    return;
                }
            }
            if (type == "client")
            {
                USR_TBL_CLIENT _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.SingleOrDefault(x => x.id == UC_client.IdClient);
                if (_client == null)
                {
                    Response.Redirect(listPage, true);
                    return;
                }
                currTbl.cl_id = _client.id;
                currTbl.cl_email = _client.contact_email;
                currTbl.cl_name_full = _client.name_full;
                currTbl.cl_name_honorific = _client.name_honorific;
                currTbl.cl_loc_country = _client.loc_country;
                currTbl.cl_pid_discount = _client.pid_discount;
                currTbl.cl_pid_lang = _client.pid_lang;
                currTbl.cl_isCompleted = _client.isCompleted;
            }
            if (type == "state")
            {
                if (currTbl.state_pid != UC_state.state_pid)
                {
                    if (UC_state.state_pid == 3 && currTbl.state_pid == 4) reservationCancelled = true;
                    if (checkAvailability() || UC_state.state_pid == 3)
                    {
                        if (currTbl.state_pid == 3 && currTbl.requestRenewal > 0)
                        {
                            currTbl.requestRenewal = -1;
                        }
                        rntUtils.rntReservation_onStateChange(currTbl);
                        currTbl.state_pid = UC_state.state_pid;
                        currTbl.state_body = UC_state.state_body;
                        currTbl.state_date = DateTime.Now;
                        currTbl.state_pid_user = UserAuthentication.CurrentUserID;
                        currTbl.state_subject = UC_state.state_subject;
                        currTbl.HAstateCancelledBy = UC_state.HAHAstateCancelledBy;
                
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('La struttura selezionata non risulta disponibile nel periodo, nessun salvataggio eseguito!')", true);
                        UC_state.IdReservation = IdReservation;
                        UC_state.IsChanged = false;
                        UC_state.FillControls();
                        return;
                    }
                }
            }
            if (type == "dt_pers")
            {
                if (checkAvailability())
                {
                    if (currTbl.dtStart != UC_dt_pers.sel_dtStart)
                    {
                        currTbl.dtIn = (DateTime?)null;
                        currTbl.is_dtStartTimeChanged = 0;
                        _isStartDateTimeChanged = true;
                    }
                    currTbl.dtStart = UC_dt_pers.sel_dtStart;
                    if (currTbl.dtEnd != UC_dt_pers.sel_dtEnd)
                    {
                        currTbl.dtOut = (DateTime?)null;
                        currTbl.is_dtEndTimeChanged = 0;
                        _isEndDateTimeChanged = true;
                    }
                    currTbl.dtEnd = UC_dt_pers.sel_dtEnd;

                    currTbl.num_adult = UC_dt_pers.sel_num_adult;
                    currTbl.num_child_over = UC_dt_pers.sel_num_child_over;
                    currTbl.num_child_min = UC_dt_pers.sel_num_child_min;
                    savePrices(ref currTbl);
                    _checkPayment = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('La struttura selezionata non risulta disponibile nel periodo, nessun salvataggio eseguito!')", true);
                    UC_dt_pers._checkCalDates = rntUtils.rntEstate_availableDatesForJSCal(UC_estate.IdEstate, IdReservation);
                    UC_dt_pers.num_persons_child = UC_estate.num_persons_child;
                    UC_dt_pers.num_persons_min = UC_estate.num_persons_min;
                    UC_dt_pers.num_persons_max = UC_estate.num_persons_max;
                    UC_dt_pers.nights_min = UC_estate.nights_min;
                    UC_dt_pers.sel_num_adult = currTbl.num_adult.objToInt32();
                    UC_dt_pers.sel_num_child_min = currTbl.num_child_min.objToInt32();
                    UC_dt_pers.sel_num_child_over = currTbl.num_child_over.objToInt32();
                    UC_dt_pers.sel_dtStart = currTbl.dtStart.Value;
                    UC_dt_pers.sel_dtEnd = currTbl.dtEnd.Value;
                    UC_dt_pers.FillControls();
                    return;
                }
            }
            if (type == "block_expire")
            {
                DateTime _blockExpire = currTbl.dtCreation.Value.AddHours(UC_block.block_expire_hours);
                if (currTbl.state_pid == 3 && currTbl.block_expire < _blockExpire)
                {
                    if (checkAvailability())
                    {
                        rntUtils.rntReservation_onStateChange(currTbl);
                        currTbl.state_pid = 6;
                        currTbl.state_body = "";
                        currTbl.state_date = DateTime.Now;
                        currTbl.state_pid_user = UserAuthentication.CurrentUserID;
                        currTbl.state_subject = "Rinnovata la scadenza";
                        if (currTbl.requestRenewal > 0)
                        {
                            currTbl.requestRenewal = -1;
                        }
                        _reload = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('La struttura selezionata non risulta disponibile nel periodo, nessun salvataggio eseguito!')", true);
                        UC_block.block_pid_user = currTbl.block_pid_user.objToInt32();
                        UC_block.block_expire_hours = currTbl.block_expire_hours.objToInt32();
                        UC_block.block_comments = currTbl.block_comments;
                        UC_block.dttCreation = currTbl.dtCreation.Value;
                        UC_block.FillControls();
                        return;
                    }
                }
                currTbl.block_expire_hours = UC_block.block_expire_hours;
                currTbl.block_expire = _blockExpire;
                currTbl.block_comments = UC_block.block_comments;
                currTbl.block_pid_user = UC_block.block_pid_user;
                _checkPayment = true;
            }
            if (type == "discount")
            {
                checkAvailability();
                savePrices(ref currTbl);
                _checkPayment = true;
            }
            if (type == "notesInner")
            {
                currTbl.notesInner = UC_notesInner.Body;
            }
            if (type == "notesEco")
            {
                currTbl.notesEco = UC_notesEco.Body;
            }
            if (type == "notesClient")
            {
                currTbl.notesClient = UC_notesClient.Body;
            }
            if (type == "problem")
            {
                currTbl.problemID = ucProblemSelect.ProblemID;
                currTbl.problemDesc = ucProblemSelect.ProblemDesc;
            }
            if (type == "agent")
            {
                if (currTbl.agentID != ucAgent.IdAgent)
                {
                    currTbl.agentID = ucAgent.IdAgent;
                    DC_RENTAL.SubmitChanges();
                    checkAvailability();
                    savePrices(ref currTbl);
                    _checkPayment = true;
                }
            }
            if (type == "agentClient")
            {
                currTbl.agentClientID = ucAgentClient.IdClient; currTbl.cl_id = -1; currTbl.cl_id = -1;
                currTbl.cl_email = ucAgentClient.cl_email;
                currTbl.cl_name_full = ucAgentClient.cl_name_full;
                currTbl.cl_name_honorific = ucAgentClient.cl_name_honorific;
                currTbl.cl_loc_country = ucAgentClient.cl_loc_country;
                DC_RENTAL.SubmitChanges();
            }

            if (type == "inout")
            {
                //_currTBL.limo_in_datetime = UC_inout.limo_in_datetime;
                //_currTBL.limo_inPoint_details = UC_inout.limo_inPoint_details;
                //_currTBL.limo_inPoint_pickupPlaceName = UC_inout.limo_inPoint_pickupPlaceName;
                //_currTBL.limo_inPoint_type = UC_inout.limo_inPoint_type;
                //_currTBL.dtStartTime = UC_inout.dtStartTime;
                //_currTBL.is_dtStartTimeChanged = 1;

                //_currTBL.limo_out_datetime = UC_inout.limo_out_datetime;
                //_currTBL.limo_outPoint_details = UC_inout.limo_outPoint_details;
                //_currTBL.limo_outPoint_pickupPlaceName = UC_inout.limo_outPoint_pickupPlaceName;
                //_currTBL.limo_outPoint_type = UC_inout.limo_outPoint_type;
                //_currTBL.dtEndTime = UC_inout.dtEndTime;
                //_currTBL.is_dtEndTimeChanged = 1;
            }
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(currTbl, _isStartDateTimeChanged, _isEndDateTimeChanged);
            if (reservationCancelled && (CommonUtilities.getSYS_SETTING("rnt_reservationCancelled_autoEmail") == "true" || CommonUtilities.getSYS_SETTING("rnt_reservationCancelled_autoEmail").ToInt32() == 1))
                rntUtils.rntReservation_mailCancelled(currTbl, false, true, true, true, true, 1); // send mails
            if (_checkPayment)
            {
                rntUtils.reservation_checkPartPayment(currTbl, false);
            }
            if (_reload)
                Response.Redirect("rnt_reservation_details_manuale.aspx?id=" + IdReservation);
        }

        protected void pricesShow(bool edit)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                Response.Redirect(listPage, true);
                return;
            }
            var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == currTbl.pid_estate);
            if (currEstate == null)
            {
                Response.Redirect(listPage, true);
                return;
            }
            ucPrice.fillData(currTbl, currEstate.pr_percentage.objToInt32(), edit);
            lnk_pricesEdit.Visible = !edit;
            lnk_pricesSave.Visible = edit;
            lnk_pricesCancel.Visible = edit;
        }
        protected void lnk_pricesEdit_Click(object sender, EventArgs e)
        {
            pricesShow(true);
        }
        protected void lnk_pricesCancel_Click(object sender, EventArgs e)
        {
            pricesShow(false);
        }
        protected void lnk_pricesSave_Click(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                Response.Redirect(listPage, true);
                return;
            }
            var tbBefore = currTbl.Clone();
            ucPrice.saveData(ref currTbl);
            DC_RENTAL.SubmitChanges();
            if (tbBefore != null) rntUtils.RNT_TBL_RESERVATION_addLog(tbBefore, currTbl, UserAuthentication.CurrentUserID, UserAuthentication.CurrentUserName);
            rntUtils.rntReservation_onChange(currTbl);
            rntUtils.reservation_checkPartPayment(currTbl, false);
            invUtils.payment_onChange(currTbl, true);
            Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation);
        }
        protected void lnk_pricesDaTariffa_Click(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                Response.Redirect(listPage, true);
                return;
            }
            var tbBefore = currTbl.Clone();
            currTbl.pr_part_modified = 0;
            DC_RENTAL.SubmitChanges();
            if (tbBefore != null) rntUtils.RNT_TBL_RESERVATION_addLog(tbBefore, currTbl, UserAuthentication.CurrentUserID, UserAuthentication.CurrentUserName);
            rntUtils.rntReservation_onChange(currTbl);
            rntUtils.reservation_checkPartPayment(currTbl, false);
            invUtils.payment_onChange(currTbl, true);
            Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation);
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
        }
        protected void lnk_sendPaymentMail_Click(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);
                return;
            }
            List<string> lstExtraOwner = chk_extrasercices.getSelectedValueList();
            rntUtils.rntReservation_mailPartPaymentReceive(currTbl, chk_mailConfirm_cl.Checked, chk_mailConfirm_ad.Checked, chk_mailConfirm_srs.Checked, chk_mailConfirm_eco.Checked, chk_mailConfirm_own.Checked, UserAuthentication.CurrentUserID); // send mails
            rntUtils.rntExtraReservation_mailPartPaymentReceive(currTbl, chk_servicemailConfirm_cl.Checked, chk_servicemailConfirm_ad.Checked, lstExtraOwner, UserAuthentication.CurrentUserID); // send extra service mails
        }
        protected void lnk_sendCreationMail_Click(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);
                return;
            }
            rntUtils.rntReservation_mailNewCreation(currTbl, true, false, false, false, UserAuthentication.CurrentUserID); // send mails
        }
        protected void lnk_requestRenewal_Click(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);
                return;
            }
            currTbl.requestRenewal = 1;
            currTbl.requestRenewalDate = DateTime.Now;
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(currTbl);
            rntUtils.rntReservation_addState(currTbl.id, 0, UserAuthentication.CurrentUserID, "Richiesta Rinnovo dell'opzione", "");
            Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation);
        }
        protected void lnk_requestFullPayAccepted_Click(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);
                return;
            }
            currTbl.requestFullPayAccepted = 1;
            currTbl.requestFullPayAcceptedDate = DateTime.Now;
            currTbl.pr_part_forPayment = currTbl.pr_total.objToDecimal(); // somma da pagare = Totale pren - sconto dell'agenzia, sconto potrebbe essere 0 se impostato dall'agenzia
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(currTbl);
            rntUtils.reservation_checkPartPayment(currTbl, false);
            rntUtils.rntReservation_addState(currTbl.id, 0, UserAuthentication.CurrentUserID, "Abilitato pagamento del Saldo", "");
            Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation);
        }
        protected void lnk_requestFullPayNotAccepted_Click(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);
                return;
            }
            currTbl.requestFullPay = 0;
            currTbl.requestFullPayAccepted = 0;
            currTbl.requestFullPayAcceptedDate = DateTime.Now;
            currTbl.pr_part_forPayment = currTbl.pr_part_payment_total.objToDecimal(); // somma da pagare = acconto calcolato, potrebbe essere gia applicato sconto dell'agenzia
            DC_RENTAL.SubmitChanges();

            rntUtils.rntReservation_onChange(currTbl);
            rntUtils.reservation_checkPartPayment(currTbl, false);
            rntUtils.rntReservation_addState(currTbl.id, 0, UserAuthentication.CurrentUserID, "Disabilitato pagamento del Saldo", "");
            Response.Redirect("rnt_reservation_details.aspx?id=" + IdReservation);
        }
        protected void lnkSendMailForComments_Click(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);
                return;
            }
            rntUtils.rntReservationMailForComments(currTbl, UserAuthentication.CurrentUserID); // send mails
        }

        protected void drp_isSendbalancePaymentEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            currTbl = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == IdReservation);
            if (currTbl == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert('Errore imprvisto contattare assistenza!')", true);
                return;
            }
            currTbl.isSendBalancePayementEmail = drp_isSendbalancePaymentEmail.getSelectedValueInt();
            DC_RENTAL.SubmitChanges();
        }


    }
}
