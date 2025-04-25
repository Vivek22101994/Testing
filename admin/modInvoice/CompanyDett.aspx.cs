using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class CompanyDett : adminBasePage
    {
        protected dbInvCompanyTBL currTBL;
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
                drp_docType_DataBind();
                drp_locCountry_DataBind();
                drp_invTaxId_DataBind(ref drp_invTaxIdAgency);
                drp_invTaxId_DataBind(ref drp_invTaxIdPrivate);
                fillData();
            }
        }
        protected void drp_invTaxId_DataBind(ref DropDownList drp)
        {
            drp.DataSource = invProps.CashTaxLK.OrderBy(x => x.id);
            drp.DataTextField = "code";
            drp.DataValueField = "id";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- - -", ""));
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
            string _folder = "images/Company";
            if (!Directory.Exists(Path.Combine(App.SRP, _folder)))
                Directory.CreateDirectory(Path.Combine(App.SRP, _folder));
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                currTBL = dc.dbInvCompanyTBLs.SingleOrDefault(x => x.id == IdOwner);
                if (currTBL == null)
                {
                    IdOwner = 0;
                    currTBL = new dbInvCompanyTBL();
                    ltrTitle.Text = "Nuova Societa fatturazione";
                }
                else
                    ltrTitle.Text = "Societa fatturazione #:" + currTBL.code;

                imgLogo.ImgPathDef = "";
                imgLogo.ImgRoot = _folder;
                imgLogo.ImgPath = currTBL.imgLogo;

                drp_isActive.setSelectedValue(currTBL.isActive);
                drp_isForReservations.setSelectedValue(currTBL.isForReservations);
                drp_isForAgencies.setSelectedValue(currTBL.isForAgencies);
                txt_nameFull.Text = currTBL.nameFull;
                txt_id_codice.Text = currTBL.idCodice;
                
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

                var taxRl = dc.dbInvCompanyTaxRLs.SingleOrDefault(x => x.pidCompany == currTBL.id && x.isPrivate == 1 && x.locCountry == "");
                drp_invTaxIdPrivate.setSelectedValue(taxRl != null ? taxRl.invTaxId : 0);
                taxRl = dc.dbInvCompanyTaxRLs.SingleOrDefault(x => x.pidCompany == currTBL.id && x.isPrivate == 0 && x.locCountry == "");
                drp_invTaxIdAgency.setSelectedValue(taxRl != null ? taxRl.invTaxId : 0);

                re_notesInner.Content = currTBL.notesInner;
                re_notesInvoice.Content = currTBL.notesInvoice;
            }
        }
        private void saveData()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {
                currTBL = dc.dbInvCompanyTBLs.SingleOrDefault(x => x.id == IdOwner);
                if (currTBL == null)
                {
                    currTBL = new dbInvCompanyTBL();
                    currTBL.uid = Guid.NewGuid();
                    currTBL.createdDate = DateTime.Now;
                    currTBL.createdUserID = authUtils.CurrentUserID;
                    currTBL.createdUserNameFull = authUtils.CurrentUserName;
                    dc.Add(currTBL);
                    dc.SaveChanges();
                    currTBL.code = currTBL.id.ToString().fillString("0", 6, false);
                    IdOwner = currTBL.id;
                }
                currTBL.isActive = drp_isActive.getSelectedValueInt();
                currTBL.isForReservations = drp_isForReservations.getSelectedValueInt();
                currTBL.isForAgencies = drp_isForAgencies.getSelectedValueInt();
                currTBL.nameFull = txt_nameFull.Text;
                currTBL.idCodice = txt_id_codice.Text;               

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

                currTBL.imgLogo = imgLogo.ImgPath;
                currTBL.notesInner = re_notesInner.Content;
                currTBL.notesInvoice = re_notesInvoice.Content;

                dc.SaveChanges();
                var taxRl = dc.dbInvCompanyTaxRLs.SingleOrDefault(x => x.pidCompany == currTBL.id && x.isPrivate == 1 && x.locCountry == "");
                if (taxRl == null)
                {
                    taxRl = new dbInvCompanyTaxRL() { pidCompany = currTBL.id, isPrivate = 1, locCountry = "" };
                    dc.Add(taxRl);
                }
                taxRl.invTaxId = drp_invTaxIdPrivate.getSelectedValueInt();
                taxRl = dc.dbInvCompanyTaxRLs.SingleOrDefault(x => x.pidCompany == currTBL.id && x.isPrivate == 0 && x.locCountry == "");
                if (taxRl == null)
                {
                    taxRl = new dbInvCompanyTaxRL() { pidCompany = currTBL.id, isPrivate = 0, locCountry = "" };
                    dc.Add(taxRl);
                }
                taxRl.invTaxId = drp_invTaxIdAgency.getSelectedValueInt();
                dc.SaveChanges();

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

