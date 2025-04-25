using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using RentalInRome.data;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraOwnerDett : adminBasePage
    {
        protected dbRntExtraOwnerTBL currTBL;
        private List<dbRntExtraOwnerLN> TMPcurrLangs;
        private List<dbRntExtraOwnerLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbRntExtraOwnerLN)).Cast<dbRntExtraOwnerLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbRntExtraOwnerLN>();

                return TMPcurrLangs;
            }
            set
            {
                ViewState["currLangs"] = PConv.SerialList(value.Cast<object>().ToList());
                TMPcurrLangs = value;
            }
        }
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
                //drp_pidDiscountType_DataBind();
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
                        using (DCmodRental dc = new DCmodRental())
                        {
                            currTBL = dc.dbRntExtraOwnerTBLs.SingleOrDefault(x => x.id == IdAgent);
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
            }
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
        //protected void drp_pidDiscountType_DataBind()
        //{
        //    drp_pidDiscountType.DataSource = rntProps.DiscountTypeTBL.OrderBy(x => x.id);
        //    drp_pidDiscountType.DataTextField = "code";
        //    drp_pidDiscountType.DataValueField = "id";
        //    drp_pidDiscountType.DataBind();
        //    drp_pidDiscountType.Items.Insert(0, new ListItem("scegli", ""));
        //}
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
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntExtraOwnerTBLs.SingleOrDefault(x => x.id == IdAgent);
                if (currTBL == null)
                {
                    IdAgent = 0;
                    currTBL = new dbRntExtraOwnerTBL();
                    ltrTitle.Text = "Nuovo Record in Agenzie";
                }
                else
                    ltrTitle.Text = "Scheda Agenzia #:" + currTBL.code;

                if (currTBL.mailSentLast.HasValue)
                    ltrMailSent.Text = "Ultimo invio: " + currTBL.mailSentLast + " (x " + currTBL.mailSentCount + ")";
                else
                    ltrMailSent.Text = "Nessuna mail inviata.";

                //string paymentType = currTBL.payDiscountNotPayed.objToInt32() + "|" + currTBL.payFullPayment.objToInt32();
                //drp_paymentType.setSelectedValue(paymentType);

                drp_isActive.setSelectedValue(currTBL.isActive.ToString());
                drp_hasAcceptedContract.setSelectedValue(currTBL.hasAcceptedContract.ToString());
                //drp_pidDiscountType.setSelectedValue(currTBL.pidDiscountType);
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
                drp_locCountry.setSelectedValue(currTBL.locCountry, "Italy");

                txt_authPwd.Text = currTBL.authPwd;
                txt_authUsr.Text = currTBL.authUsr;
                pnl_auth.Visible = UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0;

                re_notesInner.Content = currTBL.notesInner;
                re_notesInvoice.Content = currTBL.notesInvoice;
                currLangs = dc.dbRntExtraOwnerLNs.Where(x => x.pidExtraOwner == currTBL.id).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                if (dc.dbRntExtraOwnerTBLs.FirstOrDefault(x => x.contactEmail == txt_contactEmail.Text.Trim() && x.id != IdAgent) != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"L'e-mail è stata già registrata per un'altra agenzia.\", 340, 110);", true);
                    return;
                }
                currTBL = dc.dbRntExtraOwnerTBLs.SingleOrDefault(x => x.id == IdAgent);
                if (currTBL == null)
                {
                    currTBL = new dbRntExtraOwnerTBL();
                    currTBL.uid = Guid.NewGuid();
                    currTBL.createdDate = DateTime.Now;
                    currTBL.createdUserID = UserAuthentication.CurrentUserID;
                    currTBL.createdUserNameFull = UserAuthentication.CurrentUserName;
                    currTBL.nameCompany = txt_nameCompany.Text;
                    dc.Add(currTBL);
                    dc.SaveChanges();
                    currTBL.code = currTBL.id.ToString().fillString("0", 6, false);
                    IdAgent = currTBL.id;
                    currTBL.pidReferer = (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0) ? drp_pidReferer.getSelectedValueInt(0) : UserAuthentication.CurrentUserID;
                }
                else
                    currTBL.pidReferer = drp_pidReferer.getSelectedValueInt(0);

                //string paymentType = drp_paymentType.SelectedValue;
                //if (paymentType.Length == 3 && paymentType.Contains("|"))
                //{
                //    currTBL.payDiscountNotPayed = paymentType.splitStringToList("|")[0].ToInt32();
                //    currTBL.payFullPayment = paymentType.splitStringToList("|")[1].ToInt32();
                //}
                //else
                //{
                //    currTBL.payDiscountNotPayed = 0;
                //    currTBL.payFullPayment = 0;
                //}

                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.hasAcceptedContract = drp_hasAcceptedContract.getSelectedValueInt();
                //currTBL.pidDiscountType = drp_pidDiscountType.getSelectedValueInt();
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
                currTBL.locCountry = drp_locCountry.SelectedValue;

                currTBL.notesInner = re_notesInner.Content;
                currTBL.notesInvoice = re_notesInvoice.Content;

                if (UserAuthentication.CurrRoleTBL.rnt_onlyOwnedAgents.objToInt32() == 0)
                {
                    currTBL.authUsr = txt_authUsr.Text;
                    currTBL.authPwd = txt_authPwd.Text;
                }
                if (string.IsNullOrEmpty(currTBL.authUsr))
                    currTBL.authUsr = currTBL.contactEmail;
                if (string.IsNullOrEmpty(currTBL.authPwd))
                    currTBL.authPwd = string.Empty.createPassword(8, true, true, false);

                dc.SaveChanges();
                SaveAllLangs(currTBL.id.objToInt32());
                HfId.Value = Convert.ToString(currTBL.id);
                //rntProps.AgentTBL = dc.dbRntAgentTBLs.ToList();
                CloseRadWindow("reload");
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                //fillData();
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
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
        protected void BindLvLangs()
        {
            LvLangs.DataSource = contProps.LangTBL.Where(x => x.is_active == 1).OrderBy(x => x.id);
            LvLangs.DataBind();
        }
        protected void LvLangs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkLang");
            lnk.CssClass = HfLang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
        }
        protected void LvLangs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                SaveLang();
                HfLang.Value = lbl_id.Text;
                FillLang();
                BindLvLangs();
            }
        }
        protected void SaveLang()
        {
            var currLangsTmp = currLangs;
            var rlLang = currLangsTmp.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntExtraOwnerLN();
                rlLang.pidExtraOwner = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.Policy = txt_condition.Content;         
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntExtraOwnerLN();
            }
            txt_condition.Content = rlLang.Policy;
           
        }
        protected void SaveAllLangs(int id)
        {
            SaveLang();
            using (DCmodRental dc = new DCmodRental())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbRntExtraOwnerLNs.SingleOrDefault(x => x.pidExtraOwner == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidExtraOwner = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbRntExtraOwnerLNs.Single(x => x.pidExtraOwner == id && x.pidLang == rl.pidLang);
                        rl.CopyToExtraOwner(ref currLN);
                    }
                }
                dc.SaveChanges();
            }
        }
    }
}