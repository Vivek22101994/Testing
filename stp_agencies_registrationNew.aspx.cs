using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class stp_agencies_registrationNew : contStpBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "stp";
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
                ErrorLog.addLog(_ip, "stp", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
        }
        magaRental_DataContext DC_RENTAL;
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                setPlaceHolder();
                Fill_data();
                drp_pidLang_DataBind();
                Bind_drp_honorific();
            }
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCal", "setCal_" + Unique + "(" + HF_dtStart.Value + "," + HF_dtEnd.Value + ");", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "_RNT_validateBook", "$(function() {$(\"#" + txt_email.ClientID + ",#" + txt_email_conf.ClientID + "\").bind(\"cut copy paste\", function(event) { event.preventDefault();}); });", true);
        }

        protected void setPlaceHolder()
        {
            txt_nameFull.Attributes.Add("placeholder", CurrentSource.getSysLangValue("formContactNameFull") + "*");
            txt_email.Attributes.Add("placeholder", CurrentSource.getSysLangValue("reqEmail") + "*");
            txt_email_conf.Attributes.Add("placeholder", CurrentSource.getSysLangValue("reqEmailConfirm") + "*");

            txt_contactPhone.Attributes.Add("placeholder", CurrentSource.getSysLangValue("lblPhone"));
            txt_nameCompany.Attributes.Add("placeholder", CurrentSource.getSysLangValue("formParteContraente") + "*");
            txt_locAddress.Attributes.Add("placeholder", CurrentSource.getSysLangValue("lblAddress") + "*");

            txt_locCity.Attributes.Add("placeholder", CurrentSource.getSysLangValue("lblCity") + "*");
            txt_locState.Attributes.Add("placeholder", CurrentSource.getSysLangValue("locStateProvince") + "*");
            txt_locZipCode.Attributes.Add("placeholder", CurrentSource.getSysLangValue("lblZipCode") + "*");


            txt_docVat.Attributes.Add("placeholder", CurrentSource.getSysLangValue("formVatNumber") + "*");

            drp_contactComeFrom.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("formWhereDidYouHearAboutUs"), ""));

            txt_note.Attributes.Add("placeholder", CurrentSource.getSysLangValue("lblNotes"));
            txt_contactComeFrom.Attributes.Add("placeholder", CurrentSource.getSysLangValue("lblOther"));
            txt_contactWebSite.Attributes.Add("placeholder", CurrentSource.getSysLangValue("lblWebAddress"));

        }


        protected void Fill_data()
        {
        }
        protected void Bind_drp_honorific()
        {
            List<USR_LK_HONORIFIC> _listAll = maga_DataContext.DC_USER.USR_LK_HONORIFICs.ToList();
            List<USR_LK_HONORIFIC> _list = _listAll.Where(x => x.pid_lang == CurrentLang.ID).ToList();
            if (_list.Count() == 0)
                _list = _listAll.Where(x => x.pid_lang == 2).ToList();
            drp_honorific.DataSource = _list;
            drp_honorific.DataTextField = "title";
            drp_honorific.DataValueField = "title";
            drp_honorific.DataBind();
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
        protected void drp_country_DataBound(object sender, EventArgs e)
        {
            drp_country.Items.Insert(0, new ListItem(CurrentSource.getSysLangValue("reqLocation"), ""));
            if (CurrentLang.ID != 2)
            {
                LOC_LK_COUNTRY _c = maga_DataContext.DC_LOCATION.LOC_LK_COUNTRies.FirstOrDefault(
                        x => x.inner_notes.ToLower() == CurrentLang.NAME.Substring(3, 2).ToLower());
                if (_c != null)
                    drp_country.setSelectedValue(_c.id.ToString());
            }
        }
        protected void lnk_send_Click(object sender, EventArgs e)
        {
            saveRequest();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "scrollTo_sent", "$.scrollTo($(\"#" + pnl_request_sent.ClientID + "\"), 500);", true);
        }

        protected void saveRequest()
        {

            using (DCmodRental dc = new DCmodRental())
            {
                if (dc.dbRntAgentTBLs.FirstOrDefault(x => x.contactEmail == txt_email.Text.Trim()) != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"The E-mail has already been registered.<br/>Please contact <a href='mailto:agencies@rentalinrome.com'>agencies@rentalinrome.com</a> to receive login information.\", 340, 110);", true);
                    return;
                }
                dbRntAgentTBL currTBL = new dbRntAgentTBL();
                currTBL.uid = Guid.NewGuid();
                currTBL.createdDate = DateTime.Now;
                currTBL.createdUserID = 1;
                currTBL.createdUserNameFull = "System";
                dc.Add(currTBL);
                dc.SaveChanges();
                currTBL.code = currTBL.id.ToString().fillString("0", 6, false);
                currTBL.isActive = 0;
                currTBL.typeCode = "";

                currTBL.nameHonor = drp_honorific.SelectedValue;
                currTBL.nameFull = txt_nameFull.Text;
                currTBL.nameCompany = txt_nameCompany.Text;
                currTBL.pidLang = drp_pidLang.getSelectedValueInt(0);

                // loc
                currTBL.locAddress = txt_locAddress.Text;
                currTBL.locZipCode = txt_locZipCode.Text;
                currTBL.locCity = txt_locCity.Text;
                currTBL.locState = txt_locState.Text;
                currTBL.locCountry = drp_country.getSelectedText("");

                // contact
                currTBL.contactEmail = txt_email.Text.Trim();
                currTBL.contactPhone = txt_contactPhone.Text;
                currTBL.contactFax = txt_contactFax.Text;
                currTBL.contactWebSite = txt_contactWebSite.Text;
                currTBL.contactComeFrom = drp_contactComeFrom.SelectedValue == "other" ? txt_contactComeFrom.Text : drp_contactComeFrom.SelectedValue;

                // doc
                currTBL.docVat = txt_docVat.Text;
                currTBL.docVat_isEuReg = chk_docVat_isEuReg.Checked ? 1 : 0;

                currTBL.notesClient = txt_note.InnerText;

                currTBL.authUsr = currTBL.contactEmail;
                currTBL.authPwd = string.Empty.createPassword(8, true, true, false);

                dc.SaveChanges();
                pnl_request_cont.Visible = false;
                pnl_request_sent.Visible = true;

            }
        }
    }
}