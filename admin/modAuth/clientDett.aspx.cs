using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModAuth.admin.modAuth
{
    public partial class clientDett : adminBasePage
    {
        protected AuthClientTBL currTBL;
        protected long IdOwner
        {
            get { return HfId.Value.ToInt64(); }
            set { HfId.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdOwner = Request.QueryString["id"].ToInt64();
                HF_reloadDrp.Value = Request.QueryString["reloadDrp"] == "1" ? "1" : "0";
                drp_typeCode_DataBind();
                drp_docType_DataBind();
                drp_locCountry_DataBind();
                fillData();
            }
        }
        protected void drp_pidAgent_DataBind()
        {
            drp_pidAgent.DataSource = rntProps.AgentTBL.Where(x => x.isActive == 1).OrderBy(x=>x.nameCompany);
            drp_pidAgent.DataTextField = "nameCompany";
            drp_pidAgent.DataValueField = "id";
            drp_pidAgent.DataBind();
            drp_pidAgent.Items.Insert(0, new ListItem("- - -", "0"));
        }
        protected void drp_typeCode_DataBind()
        {
            drp_typeCode.DataSource = authProps.ClientTypeLK.Where(x => x.isActive == 1);
            drp_typeCode.DataTextField = "title";
            drp_typeCode.DataValueField = "code";
            drp_typeCode.DataBind();
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
        }
        private void fillData()
        {
            using (magaAuth_DataContext dc = maga_DataContext.DC_Auth)
            {
                currTBL = dc.AuthClientTBL.SingleOrDefault(x => x.id == IdOwner);
                if (currTBL == null)
                {
                    IdOwner = 0;
                    currTBL = new AuthClientTBL();
                    ltrTitle.Text = "Nuovo Record in Anagrafica";
                    currTBL.typeCode = Request.QueryString["type"];
                }
                else
                    ltrTitle.Text = "Scheda Anagrafica #:" + currTBL.code;

                drp_isActive.setSelectedValue(currTBL.isActive);
                drp_typeCode.setSelectedValue(currTBL.typeCode);
                drp_pidAgent.setSelectedValue(currTBL.pidAgent.ToString());
                txt_nameFull.Text = currTBL.nameFull;
                
                // contact
                txt_contactEmail.Text = currTBL.contactEmail;
                txt_contactPhone.Text = currTBL.contactPhone;
                txt_contactPhoneMobile.Text = currTBL.contactPhoneMobile;
                txt_contactFax.Text = currTBL.contactFax;

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


                re_notesInner.Content = currTBL.notesInner;
                re_notesInvoice.Content = currTBL.notesInvoice;
            }
        }
        private void saveData()
        {
            using (magaAuth_DataContext dc = maga_DataContext.DC_Auth)
            {
                currTBL = dc.AuthClientTBL.SingleOrDefault(x => x.id == IdOwner);
                if (currTBL == null)
                {
                    currTBL = new AuthClientTBL();
                    currTBL.uid = Guid.NewGuid();
                    currTBL.createdDate = DateTime.Now;
                    currTBL.createdUserID = authUtils.CurrentUserID;
                    currTBL.createdUserNameFull = authUtils.CurrentUserName;
                    dc.AuthClientTBL.InsertOnSubmit(currTBL);
                    dc.SubmitChanges();
                    currTBL.code = currTBL.id.ToString().fillString("0", 6, false);
                    IdOwner = currTBL.id;
                }
                currTBL.isActive = drp_isActive.getSelectedValueInt();
                currTBL.typeCode = drp_typeCode.SelectedValue;
                currTBL.pidAgent = drp_pidAgent.SelectedValue.ToInt64();
                currTBL.nameFull = txt_nameFull.Text;

                // contact
                currTBL.contactEmail = txt_contactEmail.Text;
                currTBL.contactPhone = txt_contactPhone.Text;
                currTBL.contactFax = txt_contactFax.Text;
                currTBL.contactPhoneMobile = txt_contactPhoneMobile.Text;

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

                dc.SubmitChanges();
                authProps.ClientTBL = dc.AuthClientTBL.ToList();
                if (HF_reloadDrp.Value == "1")
                {
                    CloseRadWindow(currTBL.id.ToString());
                    return;
                }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
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

    }
}

