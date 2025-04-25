using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;
using System.IO;

namespace ModRental.admin.modRental
{
    public partial class agentDett : adminBasePage
    {
        protected RntAgentTBL currTBL;
        protected long IdAgent
        {
            get { return HfId.Value.ToInt64(); }
            set { HfId.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool canView = true;
                IdAgent = Request.QueryString["id"].ToInt64();
                drp_pidReferer_DataBind();
                drp_pidDiscountType_DataBind();
                if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 1)
                {
                    drp_pidReferer.Enabled = false;
                    txt_cashDiscount.Enabled = false;
                    List<int> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.rnt_canHaveAgent == 1).Select(x => x.id).ToList();
                    if (!_list.Contains(UserAuthentication.CurrentUserID))
                    {
                        canView = false;
                    }
                    else
                    {
                        using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                        {
                            currTBL = dc.RntAgentTBL.SingleOrDefault(x => x.id == IdAgent);
                            if (currTBL != null && currTBL.pidReferer != UserAuthentication.CurrentUserID)
                            {
                                canView = false;
                            }
                        }
                    }
                }
                if (!canView)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert(\"Non hai permessi per questa pagina. Contattare l'amministratore.\");CloseRadWindow('reload');", true);
                    pnlDett.Visible = false;
                    return;
                }
                drp_docType_DataBind();
                drp_locCountry_DataBind();
                drp_honorific_DataBind();
                drp_pidLang_DataBind();
                fillData();
                pnlSuperAdmin.Visible = UserAuthentication.CURRENT_USER_ROLE == 1;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setColor", "setColorPicker();", true);
        }
        protected void drp_pidReferer_DataBind()
        {
            drp_pidReferer.Items.Clear();
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.rnt_canHaveAgent == 1).OrderBy(x => x.name).ToList();
            foreach (USR_ADMIN _admin in _list)
            {
                drp_pidReferer.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
            drp_pidReferer.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        protected void drp_pidDiscountType_DataBind()
        {
            drp_pidDiscountType.DataSource = rntProps.DiscountTypeTBL.OrderBy(x => x.id);
            drp_pidDiscountType.DataTextField = "code";
            drp_pidDiscountType.DataValueField = "id";
            drp_pidDiscountType.DataBind();
            drp_pidDiscountType.Items.Insert(0, new ListItem("scegli", ""));
        }
        protected void drp_honorific_DataBind()
        {
            List<USR_LK_HONORIFIC> _listAll = maga_DataContext.DC_USER.USR_LK_HONORIFICs.ToList();
            drp_honorific.DataSource = _listAll;
            drp_honorific.DataTextField = "title";
            drp_honorific.DataValueField = "title";
            drp_honorific.DataBind();
        }
        protected void drp_docType_DataBind()
        {
            drp_docType.DataSource = authProps.DocTypeLK.OrderBy(x => x.title);
            drp_docType.DataTextField = "title";
            drp_docType.DataValueField = "code";
            drp_docType.DataBind();
        }
        protected void drp_locCountry_DataBind()
        {
            drp_locCountry.DataSource = authProps.CountryLK.OrderBy(x => x.title);
            drp_locCountry.DataTextField = "title";
            drp_locCountry.DataValueField = "title";
            drp_locCountry.DataBind();
            drp_locCountry.Items.Insert(0, "");
        }
        protected void drp_pidLang_DataBind()
        {
            drp_pidLang.DataSource = contProps.LangTBL.Where(x => x.is_active == 1 && x.is_public == 1).OrderBy(x => x.lang_title);
            drp_pidLang.DataTextField = "lang_title";
            drp_pidLang.DataValueField = "id";
            drp_pidLang.DataBind();
            drp_pidLang.Items.Insert(0, new ListItem("- - -", "0"));
            drp_pidLang.setSelectedValue(CurrentLang.ID.ToString());
        }
        private void fillData()
        {
            string _folder = "images/agentLogo";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));

            string _wlfolder = "images/agentWLLogo";
            if (!Directory.Exists(Path.Combine(App.SRP, _wlfolder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _wlfolder));

            string _wlMapfolder = "images/agentWLMap";
            if (!Directory.Exists(Path.Combine(App.SRP, _wlMapfolder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _wlMapfolder));

            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                currTBL = dc.RntAgentTBL.SingleOrDefault(x => x.id == IdAgent);
                if (currTBL == null)
                {
                    IdAgent = 0;
                    currTBL = new RntAgentTBL();
                    ltrTitle.Text = "Nuovo Record in Agenzie";
                }
                else
                    ltrTitle.Text = "Scheda Agenzia #:" + currTBL.code;

                pnlAirbnb.Visible = (currTBL.IdAdMedia + "").ToLower() == "airbnb";
                pnlHomeAway.Visible = (currTBL.IdAdMedia + "").ToLower() == "ha";
                pnlAtraveo.Visible = (currTBL.IdAdMedia + "").ToLower() == "atraveo";
                pnlRentalsUnited.Visible = (currTBL.IdAdMedia + "").ToLower() == "ru";
                pnlExpedia.Visible = (currTBL.IdAdMedia + "").ToLower() == "expedia";

                if (currTBL.mailSentLast.HasValue)
                    ltrMailSent.Text = "Ultimo invio: " + currTBL.mailSentLast + " (x " + currTBL.mailSentCount + ")";
                else
                    ltrMailSent.Text = "Nessuna mail inviata.";

                string paymentType = currTBL.payDiscountNotPayed.objToInt32() + "|" + currTBL.payFullPayment.objToInt32();
                drp_paymentType.setSelectedValue(paymentType);

                drp_isActive.setSelectedValue(currTBL.isActive.ToString());
                drp_hasAcceptedContract.setSelectedValue(currTBL.hasAcceptedContract.ToString());
                drp_pidDiscountType.setSelectedValue(currTBL.pidDiscountType);
                drp_honorific.setSelectedValue(currTBL.nameHonor);
                txt_nameFull.Text = currTBL.nameFull;
                txt_nameCompany.Text = currTBL.nameCompany;
                drp_pidLang.setSelectedValue(currTBL.pidLang.ToString());
                drp_pidReferer.setSelectedValue(UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0 ? currTBL.pidReferer.ToString() : UserAuthentication.CurrentUserID.ToString());
                txt_cashDiscount.Value = Convert.ToDouble(currTBL.cashDiscount);

                // contact
                txt_contactEmail.Text = currTBL.contactEmail;
                txt_contactPhone.Text = currTBL.contactPhone;
                txt_contactFax.Text = currTBL.contactFax;
                txt_contactWebSite.Text = currTBL.contactWebSite;
                txt_contactComeFrom.Text = currTBL.contactComeFrom;

                // doc
                drp_docType.setSelectedValue(currTBL.docType);
                txt_docNum.Text = currTBL.docNum;
                txt_docIssuePlace.Text = currTBL.docIssuePlace;
                rdp_docIssueDate.SelectedDate = currTBL.docIssueDate;
                rdp_docExpiryDate.SelectedDate = currTBL.docExpiryDate;
                txt_docVat.Text = currTBL.docVat;
                txt_docCf.Text = currTBL.docCf;
                rdp_birthDate.SelectedDate = currTBL.birthDate;
                txt_birthPlace.Text = currTBL.birthPlace;

                // loc
                txt_locAddress.Text = currTBL.locAddress;
                txt_locZipCode.Text = currTBL.locZipCode;
                txt_locCity.Text = currTBL.locCity;
                txt_locState.Text = currTBL.locState;
                txt_loc_province.Text = currTBL.cl_loc_province;
                drp_locCountry.setSelectedValue(currTBL.locCountry, "Italy");

                txt_authPwd.Text = currTBL.authPwd;
                txt_authUsr.Text = currTBL.authUsr;
                pnl_auth.Visible = UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0;

                drp_invCompanyId.setSelectedValue(currTBL.invCompanyId);
                drp_invTaxId.setSelectedValue(currTBL.invTaxId);
                drp_isMsgsEnabled.setSelectedValue(currTBL.isMsgsEnabled);
                drp_isSendMannualEmail.setSelectedValue(currTBL.isSendManualEmail.objToInt32());
                drp_isAgencyFeeApplied.setSelectedValue(currTBL.isAgencyFeeApplied);
                txt_IdAdMedia.Text = currTBL.IdAdMedia;

                txt_uid.Text = currTBL.uid + ""; 
                drp_chnlMGetPhotos.setSelectedValue(currTBL.chnlMGetPhotos);
                drp_chnlMGetTexts.setSelectedValue(currTBL.chnlMGetTexts);
                drp_chnlMGetAmenities.setSelectedValue(currTBL.chnlMGetAmenities);
                drp_chnlMGetAddress.setSelectedValue(currTBL.chnlMGetAddress);                

                drp_fattura.setSelectedValue(currTBL.Fattura);
                drp_customerPaysAgency.SelectedValue = Convert.ToString(currTBL.IsCustomerPaysAgency);
                drp_voucher.SelectedValue = Convert.ToString(currTBL.Voucher);
                drp_sentVoucherToOwner.setSelectedValue(currTBL.sentVoucherToOwner);

                imgLogo.ImgPathDef = "";
                imgLogo.ImgRoot = _folder;
                imgLogo.ImgPath = currTBL.imgLogo;

                //before payment
                if (currTBL.IsPaymentBefore != null)
                {
                    drp_paymentBefore.SelectedValue = Convert.ToString(currTBL.IsPaymentBefore);
                    if (drp_paymentBefore.SelectedValue == "1")
                    {
                        txt_days.Enabled = true;
                    }
                    else
                    {
                        txt_days.Enabled = false;
                    }
                    txt_days.Text = Convert.ToString(currTBL.Days);
                }


                //invoice complete
                if (currTBL.IsInvoiceComplete != null)
                {
                    drp_ivoiceComplete.SelectedValue = Convert.ToString(currTBL.IsInvoiceComplete);
                    if (drp_ivoiceComplete.SelectedValue == "1")
                    {
                        txt_invoicePercentage.Enabled = false;
                    }
                    else
                    {
                        txt_invoicePercentage.Enabled = true;
                    }
                    txt_invoicePercentage.Value = Convert.ToDouble(currTBL.InvoicePercentage);
                }

                LvContracts_DataBind();


                re_notesInner.Content = currTBL.notesInner;
                re_notesInvoice.Content = currTBL.notesInvoice;

                var discountLimite = dc.RntAgentPriceChangeLimitTBLs.FirstOrDefault(x => x.pidAgent == IdAgent);
                if (discountLimite != null)
                {
                    ntxt_limite_numPersons.Value = discountLimite.numPersons;
                    ntxt_limite_discountLimit.Value = discountLimite.discountLimit;
                }

                #region White Label Details
                txt_wl_name.Text = currTBL.WL_name;
             
                img_wl_logo.ImgPathDef = "";
                img_wl_logo.ImgRoot = _wlfolder;
                img_wl_logo.ImgPath = currTBL.WL_logo;

                img_wl_mapmarker.ImgPathDef = "";
                img_wl_mapmarker.ImgRoot = _wlMapfolder;
                img_wl_mapmarker.ImgPath = currTBL.WL_mapMarker;

                HF_picker_mainColor.Value = currTBL.WL_mainColor;
                HF_picker_supportColor.Value = currTBL.WL_supportColor;

                //currTBL.WL_changeIsPercentage = 1;
                ntxt_wl_changeAmount.Value = currTBL.WL_changeAmount.objToInt32();
                drp_wl_changeIsDiscount.setSelectedValue(currTBL.WL_changeIsDiscount);

                drp_wl_css.setSelectedValue(currTBL.WL_css);
                #endregion
            }
        }
        private void saveData()
        {
            
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                if (dc.RntAgentTBL.FirstOrDefault(x => x.contactEmail == txt_contactEmail.Text.Trim() && x.id != IdAgent) != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"L'e-mail è stata già registrata per un'altra agenzia.\", 340, 110);", true);
                    return;
                }
                currTBL = dc.RntAgentTBL.SingleOrDefault(x => x.id == IdAgent);
                if (currTBL == null)
                {
                    currTBL = new RntAgentTBL();
                    currTBL.uid = Guid.NewGuid();
                    currTBL.createdDate = DateTime.Now;
                    currTBL.createdUserID = UserAuthentication.CurrentUserID;
                    currTBL.createdUserNameFull = UserAuthentication.CurrentUserName;
                    currTBL.nameCompany = txt_nameCompany.Text;
                    dc.RntAgentTBL.InsertOnSubmit(currTBL);
                    dc.SubmitChanges();
                    currTBL.code = currTBL.id.ToString().fillString("0", 6, false);
                    IdAgent = currTBL.id;
                    currTBL.pidReferer = (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0) ? drp_pidReferer.getSelectedValueInt(0) : UserAuthentication.CurrentUserID;
                }
                else
                    currTBL.pidReferer = drp_pidReferer.getSelectedValueInt(0);

                string paymentType = drp_paymentType.SelectedValue;
                if (paymentType.Length == 3 && paymentType.Contains("|"))
                {
                    currTBL.payDiscountNotPayed = paymentType.splitStringToList("|")[0].ToInt32();
                    currTBL.payFullPayment = paymentType.splitStringToList("|")[1].ToInt32();
                }
                else
                {
                    currTBL.payDiscountNotPayed = 0;
                    currTBL.payFullPayment = 0;
                }

                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.hasAcceptedContract = drp_hasAcceptedContract.getSelectedValueInt();
                currTBL.pidDiscountType = drp_pidDiscountType.getSelectedValueInt();
                currTBL.nameHonor = drp_honorific.SelectedValue;
                currTBL.nameFull = txt_nameFull.Text;
                currTBL.nameCompany = txt_nameCompany.Text;
                currTBL.pidLang = drp_pidLang.getSelectedValueInt(0);
                currTBL.cashDiscount = txt_cashDiscount.Value.objToDecimal();
                
                // contact
                currTBL.contactEmail = txt_contactEmail.Text.Trim();
                currTBL.contactPhone = txt_contactPhone.Text;
                currTBL.contactFax = txt_contactFax.Text;
                currTBL.contactWebSite = txt_contactWebSite.Text;
                currTBL.contactComeFrom = txt_contactComeFrom.Text;

                // doc
                currTBL.docType = drp_docType.SelectedValue;
                currTBL.docNum = txt_docNum.Text;
                currTBL.docIssuePlace = txt_docIssuePlace.Text;
                currTBL.docIssueDate = rdp_docIssueDate.SelectedDate;
                currTBL.docExpiryDate = rdp_docExpiryDate.SelectedDate;
                currTBL.docVat = txt_docVat.Text;
                currTBL.docCf = txt_docCf.Text;
                currTBL.birthDate = rdp_birthDate.SelectedDate;
                currTBL.birthPlace = txt_birthPlace.Text;

                // loc
                currTBL.locAddress = txt_locAddress.Text;
                currTBL.locZipCode = txt_locZipCode.Text;
                currTBL.locCity = txt_locCity.Text;
                currTBL.locState = txt_locState.Text;
                currTBL.cl_loc_province = txt_loc_province.Text;
                currTBL.locCountry = drp_locCountry.SelectedValue;

                currTBL.invCompanyId = drp_invCompanyId.getSelectedValueInt();
                currTBL.invTaxId = drp_invTaxId.getSelectedValueInt();
                currTBL.isSendManualEmail = drp_isSendMannualEmail.getSelectedValueInt();
                currTBL.isMsgsEnabled = drp_isMsgsEnabled.getSelectedValueInt();
                currTBL.isAgencyFeeApplied = drp_isAgencyFeeApplied.getSelectedValueInt();
                
                currTBL.IdAdMedia = txt_IdAdMedia.Text;
                currTBL.chnlMGetPhotos = drp_chnlMGetPhotos.getSelectedValueInt();
                currTBL.chnlMGetTexts = drp_chnlMGetTexts.getSelectedValueInt();
                currTBL.chnlMGetAddress = drp_chnlMGetAddress.getSelectedValueInt();
                currTBL.chnlMGetAmenities = drp_chnlMGetAmenities.getSelectedValueInt();

                currTBL.notesInner = re_notesInner.Content;
                currTBL.notesInvoice = re_notesInvoice.Content;

                currTBL.imgLogo = imgLogo.ImgPath;

                if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0) 
                {
                    currTBL.authUsr = txt_authUsr.Text;
                    currTBL.authPwd = txt_authPwd.Text;
                }
                if (string.IsNullOrEmpty(currTBL.authUsr))
                    currTBL.authUsr = currTBL.contactEmail;
                if (string.IsNullOrEmpty(currTBL.authPwd))
                    currTBL.authPwd = string.Empty.createPassword(8, true, true, false);


                currTBL.Fattura = drp_fattura.getSelectedValueInt();
                currTBL.IsCustomerPaysAgency = Convert.ToInt32(drp_customerPaysAgency.SelectedValue);
                currTBL.Voucher = Convert.ToInt32(drp_voucher.SelectedValue);
                currTBL.sentVoucherToOwner = drp_sentVoucherToOwner.getSelectedValueInt();

                //before payment
                currTBL.IsPaymentBefore = Convert.ToInt32(drp_paymentBefore.SelectedValue);
                if (drp_paymentBefore.SelectedValue == "1")
                {
                    currTBL.Days = Convert.ToInt32(txt_days.Text);
                }
                else
                {
                    currTBL.Days = null;
                }


                //invoice complete
                currTBL.IsInvoiceComplete = Convert.ToInt32(drp_ivoiceComplete.SelectedValue);
                if (drp_ivoiceComplete.SelectedValue == "1")
                {
                    currTBL.InvoicePercentage = null;
                }
                else
                {
                    currTBL.InvoicePercentage = txt_invoicePercentage.Value.objToDecimal();
                }

                dc.SubmitChanges();

                var discountLimite = dc.RntAgentPriceChangeLimitTBLs.FirstOrDefault(x => x.pidAgent == IdAgent);
                if (discountLimite == null)
                {
                    discountLimite = new RntAgentPriceChangeLimitTBL();
                    discountLimite.pidAgent = IdAgent;
                    dc.RntAgentPriceChangeLimitTBLs.InsertOnSubmit(discountLimite);
                }
                discountLimite.numPersons = ntxt_limite_numPersons.Value.objToInt32();
                discountLimite.discountLimit = ntxt_limite_discountLimit.Value.objToInt32();

                #region White Label Details
                currTBL.WL_name = txt_wl_name.Text;
                currTBL.WL_logo = img_wl_logo.ImgPath;
                currTBL.WL_mapMarker = img_wl_mapmarker.ImgPath;

                currTBL.WL_mainColor = HF_picker_mainColor.Value;
                currTBL.WL_supportColor = HF_picker_supportColor.Value;

                currTBL.WL_changeIsPercentage = 1;
                currTBL.WL_changeAmount = ntxt_wl_changeAmount.Value.objToInt32();
                currTBL.WL_changeIsDiscount = drp_wl_changeIsDiscount.getSelectedValueInt();

                currTBL.WL_css = drp_wl_css.SelectedValue;
                #endregion

                dc.SubmitChanges();
                IdAgent = currTBL.id;
                using (DCmodRental dcnew = new DCmodRental())
                {
                    rntProps.AgentTBL = dcnew.dbRntAgentTBLs.ToList();
                }                
                //CloseRadWindow("reload");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            }
        }

        protected void drp_paymentBefore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drp_paymentBefore.SelectedValue == "1")
            {
                txt_days.Enabled = true;
            }
            else
            {
                txt_days.Enabled = false;
            }
        }


        protected void drp_ivoiceComplete_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drp_ivoiceComplete.SelectedValue == "1")
            {
                txt_invoicePercentage.Enabled = false;
            }
            else
            {
                txt_invoicePercentage.Enabled = true;
            }
        }
        protected void lnkNewContract_Click(object sender, EventArgs e)
        {
            Response.Redirect("agentContractDett.aspx?id=0&idagent=" + IdAgent);
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
        }
        protected void LvContracts_DataBind()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                LvContracts.DataSource = dc.dbRntAgentContractTBLs.Where(x => x.pidAgent == IdAgent).ToList();
                LvContracts.DataBind();
            }
        }
        protected void LvContracts_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");

                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var currTBLDelete = dc.RntAgentContractTBL.SingleOrDefault(x => x.id == lbl_id.Text.ToInt64());
                    if (currTBLDelete != null)
                    {
                        dc.RntAgentContractTBL.DeleteOnSubmit(currTBLDelete);
                        dc.SubmitChanges();
                    }

                }
                LvContracts_DataBind();

            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected void lnkSendMail_Click(object sender, EventArgs e)
        {
            rntUtils.agent_mailNewCreation(IdAgent);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Procedura avviata, potrebbe richiedere alcuni minuti.\", 340, 110);", true);
        }

    }
}

