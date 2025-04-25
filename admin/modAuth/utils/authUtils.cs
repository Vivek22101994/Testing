using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ModAuth;
using System.Globalization;
using System.Threading;
using RentalInRome.data;

public class authUtils
{
    public static USR_TBL_CLIENT getClientFromAgent(long id)
    {
        USR_TBL_CLIENT tmpClient = new USR_TBL_CLIENT();
        var agClient = authProps.ClientTBL.SingleOrDefault(x => x.id == id);
        if (agClient == null)
            return tmpClient;

        tmpClient.pid_lang = agClient.pidLang.objToInt32();
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
    public static string getDocType_title(string code, string alternate)
    {
        var tmp = authProps.DocTypeLK.SingleOrDefault(x => x.code == code);
        if (tmp != null)
            return tmp.title;
        return alternate;
    }
    public static string getRole_title(int id, string alternate)
    {
        dbAuthRoleTBL tmp = authProps.RoleTBL.SingleOrDefault(x => x.id == id);
        if (tmp != null)
            return tmp.title;
        return alternate;
    }
    public static string getClient_nameFull(int id, string alternate)
    {
        using (DCmodAuth dc = new DCmodAuth())
        {
            dbAuthClientTBL tmp = dc.dbAuthClientTBLs.SingleOrDefault(x => x.id == id);
            if (tmp != null)
                return tmp.nameFull;
            return alternate;
        }
    }
    public static string getClient_email(int id, string alternate)
    {
        using (DCmodAuth dc = new DCmodAuth())
        {
            dbAuthClientTBL tmp = dc.dbAuthClientTBLs.SingleOrDefault(x => x.id == id);
            if (tmp != null)
                return tmp.contactEmail;
            return alternate;
        }
    }
    public static string getUserReportType_title(string code, string alternate)
    {
        authExts.userReportType tmp = authProps.UserReportType.SingleOrDefault(x => x.code == code);
        if (tmp != null)
            return tmp.title;
        return alternate;
    }
    public static bool hasPermission(string code)
    {
        return code == "" || (code == "superadmin" && currRole.id == 1) || currRolePermissionList.FirstOrDefault(x => x.permission == code) != null;
    }
    public static bool hasPermission(string code, authExts.authMode mode)
    {
        dbAuthRolePermissionTBL permission = currRolePermissionList.FirstOrDefault(x => x.permission == code);
        if (permission == null) return false;
        if (mode.mode == "canEdit") return permission.canEdit == 1;
        if (mode.mode == "canCreate") return permission.canCreate == 1;
        if (mode.mode == "canDelete") return permission.canDelete == 1;
        return false;
    }
    public static bool Authenticate( string login, string password)
    {
        using (DCmodAuth dc = new DCmodAuth())
        {
            dbAuthUserTBL user = dc.dbAuthUserTBLs.SingleOrDefault(x => x.login == login && x.password == password && x.isActive == 1);
            if (user != null)
            {
                HttpContext.Current.Session.Timeout = 60;
                CurrentUserID = user.id;
                CurrentUserName = user.nameFull;
                dbAuthRoleTBL TMPcurrRole = authProps.RoleTBL.SingleOrDefault(x => x.id == user.pidRole);
                if (TMPcurrRole == null)
                {
                    TMPcurrRole = new dbAuthRoleTBL();
                    TMPcurrRole.id = -1;
                }
                currRolePermissionList = authProps.RolePermissionTBL.Where(x => x.pidRole == TMPcurrRole.id).ToList();
                currRole = TMPcurrRole;
                return true;
            }
        }
        return false;
    }
    public static List<dbAuthRolePermissionTBL> currRolePermissionList
    {
        get
        {
            if (HttpContext.Current.Session["currRolePermissionList"] == null)
            {
                List<dbAuthRolePermissionTBL> tmpList = authProps.RolePermissionTBL.Where(x => x.pidRole == currRole.id).ToList();
                HttpContext.Current.Session["currRolePermissionList"] = tmpList;
            }
            return (List<dbAuthRolePermissionTBL>)HttpContext.Current.Session["currRolePermissionList"];
        }
        set
        {
            HttpContext.Current.Session["currRolePermissionList"] = value;
        }
    }
    public static dbAuthRoleTBL currRole
    {
        get
        {
            if (HttpContext.Current.Session["CURRENT_USER_ROLE"] == null)
            {
                return new dbAuthRoleTBL();
            }
            return (dbAuthRoleTBL)HttpContext.Current.Session["CURRENT_USER_ROLE"];
        }
        set
        {
            HttpContext.Current.Session["CURRENT_USER_ROLE"] = value;
        }
    }

    public static int CurrentUserID
    {
        get { if (HttpContext.Current.Session["CurrentUserID_adminMilleniumBeB"] != null)return (int)HttpContext.Current.Session["CurrentUserID_adminMilleniumBeB"]; else return 0; }
        set { HttpContext.Current.Session["CurrentUserID_adminMilleniumBeB"] = value; }
    }
    public static string CurrentUserName
    {
        get { if (HttpContext.Current.Session["CurrentUserName_adminMilleniumBeB"] != null)return (string)HttpContext.Current.Session["CurrentUserName_adminMilleniumBeB"]; else return ""; }
        set { HttpContext.Current.Session["CurrentUserName_adminMilleniumBeB"] = value; }
    }
}
public class authModes
{
    public static authExts.authMode CanEdit { get { return new authExts.authMode("canEdit"); } }
    public static authExts.authMode CanCreate { get { return new authExts.authMode("canCreate"); } }
    public static authExts.authMode CanDelete { get { return new authExts.authMode("canDelete"); } }
}
public class authProps
{
    private static List<dbAuthClientTypeLK> tmpClientTypeLK; // refresh Auto
    public static List<dbAuthClientTypeLK> ClientTypeLK
    {
        get
        {
            if (tmpClientTypeLK == null)
            {
                using (DCmodAuth dc = new DCmodAuth())
                    tmpClientTypeLK = dc.dbAuthClientTypeLKs.ToList();
            }
            return new List<dbAuthClientTypeLK>(tmpClientTypeLK.Select(x => x.Clone()));
        }
        set { tmpClientTypeLK = value; }
    }
    private static List<AuthClientTBL> tmpClientTBL; // refresh Auto
    public static List<AuthClientTBL> ClientTBL
    {
        get
        {
            if (tmpClientTBL == null)
            {
                using (magaAuth_DataContext dc = maga_DataContext.DC_Auth)
                    tmpClientTBL = dc.AuthClientTBL.ToList();
            }
            return new List<AuthClientTBL>(tmpClientTBL.Select(x => x.Clone()));
        }
        set { tmpClientTBL = value; }
    }
    private static List<dbAuthPermissionLK> tmpPermissionLK; // refresh Auto
    public static List<dbAuthPermissionLK> PermissionLK
    {
        get
        {
            if (tmpPermissionLK == null)
            {
                using (DCmodAuth dc = new DCmodAuth())
                    tmpPermissionLK = dc.dbAuthPermissionLKs.ToList();
            }
            return new List<dbAuthPermissionLK>(tmpPermissionLK.Select(x => x.Clone()));
        }
        set { tmpPermissionLK = value; }
    }
    private static List<dbAuthRoleTBL> tmpRoleTBL; // refresh Auto
    public static List<dbAuthRoleTBL> RoleTBL
    {
        get
        {
            if (tmpRoleTBL == null)
            {
                using (DCmodAuth dc = new DCmodAuth())
                    tmpRoleTBL = dc.dbAuthRoleTBLs.ToList();
            }
            return new List<dbAuthRoleTBL>(tmpRoleTBL.Select(x => x.Clone()));
        }
        set { tmpRoleTBL = value; }
    }
    private static List<dbAuthRolePermissionTBL> tmpRolePermissionTBL; // refresh Auto
    public static List<dbAuthRolePermissionTBL> RolePermissionTBL
    {
        get
        {
            if (tmpRolePermissionTBL == null)
            {
                using (DCmodAuth dc = new DCmodAuth())
                    tmpRolePermissionTBL = dc.dbAuthRolePermissionTBLs.ToList();
            }
            return new List<dbAuthRolePermissionTBL>(tmpRolePermissionTBL.Select(x => x.Clone()));
        }
        set { tmpRolePermissionTBL = value; }
    }
    private static List<dbAuthDocTypeLK> tmpDocTypeLK; // refresh Auto
    public static List<dbAuthDocTypeLK> DocTypeLK
    {
        get
        {
            if (tmpDocTypeLK == null)
            {
                using (DCmodAuth dc = new DCmodAuth())
                    tmpDocTypeLK = dc.dbAuthDocTypeLKs.ToList();
            }
            return new List<dbAuthDocTypeLK>(tmpDocTypeLK.Select(x => x.Clone()));
        }
        set { tmpDocTypeLK = value; }
    }
    private static List<dbAuthCountryLK> tmpCountryLK; // refresh Auto
    public static List<dbAuthCountryLK> CountryLK
    {
        get
        {
            if (tmpCountryLK == null)
            {
                using (DCmodAuth dc = new DCmodAuth())
                    tmpCountryLK = dc.dbAuthCountryLKs.ToList();
            }
            return new List<dbAuthCountryLK>(tmpCountryLK.Select(x => x.Clone()));
        }
        set { tmpCountryLK = value; }
    }
    private static List<authExts.userReportType> tmpUserReportType; // refresh Auto
    public static List<authExts.userReportType> UserReportType
    {
        get
        {
            if (tmpUserReportType == null)
            {
                List<authExts.userReportType> tmp = new List<authExts.userReportType>();
                tmp.Add(new authExts.userReportType("social", "Social Network"));
                tmp.Add(new authExts.userReportType("blog", "Articolo Blog"));
                tmp.Add(new authExts.userReportType("agent", "Contatto Agenzia"));
                tmp.Add(new authExts.userReportType("other", "Altro"));
                tmpUserReportType = tmp;
            }
            return new List<authExts.userReportType>(tmpUserReportType);
        }
        set { tmpUserReportType = value; }
    }
}
public static class authExts
{
    public static dbAuthCountryLK Clone(this dbAuthCountryLK source)
    {
        dbAuthCountryLK clone = new dbAuthCountryLK();
        clone.id = source.id;
        clone.code = source.code;
        clone.title = source.title;
        return clone;
    }
    public static dbAuthDocTypeLK Clone(this dbAuthDocTypeLK source)
    {
        dbAuthDocTypeLK clone = new dbAuthDocTypeLK();
        clone.code = source.code;
        clone.title = source.title;
        clone.notesInner = source.notesInner;
        return clone;
    }
    public static dbAuthPermissionLK Clone(this dbAuthPermissionLK source)
    {
        dbAuthPermissionLK clone = new dbAuthPermissionLK();
        clone.code = source.code;
        clone.title = source.title;
        clone.notesInner = source.notesInner;
        return clone;
    }
    public static dbAuthRoleTBL Clone(this dbAuthRoleTBL source)
    {
        dbAuthRoleTBL clone = new dbAuthRoleTBL();
        clone.id = source.id;
        clone.title = source.title;
        clone.notesInner = source.notesInner;
        return clone;
    }
    public static dbAuthRolePermissionTBL Clone(this dbAuthRolePermissionTBL source)
    {
        dbAuthRolePermissionTBL clone = new dbAuthRolePermissionTBL();
        clone.pidRole = source.pidRole;
        clone.permission = source.permission;
        clone.canEdit = source.canEdit;
        clone.canCreate = source.canCreate;
        clone.canDelete = source.canDelete;
        return clone;
    }
    public class authMode
    {
        public string mode { get; set; }
        public authMode(string Mode)
        {
            mode = Mode;
        }
    }
    public class userReportType
    {
        public string code { get; set; }
        public string title { get; set; }
        public userReportType(string Code, string Title)
        {
            code = Code;
            title = Title;
        }
    }

    #region Client
    public static AuthClientTBL Clone(this AuthClientTBL source)
    {
        AuthClientTBL clone = new AuthClientTBL();
        clone.id = source.id;
        clone.uid = source.uid;
        clone.code = source.code;
        clone.typeCode = source.typeCode;
        clone.pidLang = source.pidLang;
        clone.authUsr = source.authUsr;
        clone.authPwd = source.authPwd;
        clone.nameHonorific = source.nameHonorific;
        clone.nameFull = source.nameFull;
        clone.nameFirst = source.nameFirst;
        clone.nameMiddle = source.nameMiddle;
        clone.nameLast = source.nameLast;
        clone.contactEmail = source.contactEmail;
        clone.contactPhone = source.contactPhone;
        clone.contactPhoneTrip = source.contactPhoneTrip;
        clone.contactPhoneMobile = source.contactPhoneMobile;
        clone.contactPhoneOffice = source.contactPhoneOffice;
        clone.contactFax = source.contactFax;
        clone.docType = source.docType;
        clone.docNum = source.docNum;
        clone.docIssuePlace = source.docIssuePlace;
        clone.docIssueDate = source.docIssueDate;
        clone.docExpiryDate = source.docExpiryDate;
        clone.docVat = source.docVat;
        clone.docCf = source.docCf;
        clone.birthDate = source.birthDate;
        clone.birthPlace = source.birthPlace;
        clone.birthCountry = source.birthCountry;
        clone.birthState = source.birthState;
        clone.locCountry = source.locCountry;
        clone.locState = source.locState;
        clone.locCity = source.locCity;
        clone.locAddress = source.locAddress;
        clone.locZipCode = source.locZipCode;
        clone.isActive = source.isActive;
        clone.createdDate = source.createdDate;
        clone.createdUserID = source.createdUserID;
        clone.createdUserNameFull = source.createdUserNameFull;
        clone.notesInner = source.notesInner;
        clone.notesInvoice = source.notesInvoice;
        return clone;
    }
    public static void CopyTo(this dbAuthClientTBL source, ref dbAuthClientTBL copyto)
    {
        copyto.typeCode = source.typeCode;
        copyto.pidLang = source.pidLang;
        copyto.authUsr = source.authUsr;
        copyto.authPwd = source.authPwd;
        copyto.nameHonorific = source.nameHonorific;
        copyto.nameFull = source.nameFull;
        copyto.nameFirst = source.nameFirst;
        copyto.nameMiddle = source.nameMiddle;
        copyto.nameLast = source.nameLast;
        copyto.contactEmail = source.contactEmail;
        copyto.contactPhone = source.contactPhone;
        copyto.contactPhoneTrip = source.contactPhoneTrip;
        copyto.contactPhoneMobile = source.contactPhoneMobile;
        copyto.contactPhoneOffice = source.contactPhoneOffice;
        copyto.contactFax = source.contactFax;
        copyto.docType = source.docType;
        copyto.docNum = source.docNum;
        copyto.docIssuePlace = source.docIssuePlace;
        copyto.docIssueDate = source.docIssueDate;
        copyto.docExpiryDate = source.docExpiryDate;
        copyto.docVat = source.docVat;
        copyto.docCf = source.docCf;
        copyto.birthDate = source.birthDate;
        copyto.birthPlace = source.birthPlace;
        copyto.birthCountry = source.birthCountry;
        copyto.birthState = source.birthState;
        copyto.locCountry = source.locCountry;
        copyto.locState = source.locState;
        copyto.locCity = source.locCity;
        copyto.locAddress = source.locAddress;
        copyto.locZipCode = source.locZipCode;
        copyto.isActive = source.isActive;
        copyto.createdDate = source.createdDate;
        copyto.createdUserID = source.createdUserID;
        copyto.createdUserNameFull = source.createdUserNameFull;
        copyto.notesInner = source.notesInner;
        copyto.notesInvoice = source.notesInvoice;
    }
    public static bool CompareTo(this dbAuthClientTBL source, ref dbAuthClientTBL compareto)
    {
        return 1 == 1
            && compareto.typeCode == source.typeCode
            && compareto.pidLang == source.pidLang
            && compareto.authUsr == source.authUsr
            && compareto.authPwd == source.authPwd
            && compareto.nameHonorific == source.nameHonorific
            && compareto.nameFull == source.nameFull
            && compareto.nameFirst == source.nameFirst
            && compareto.nameMiddle == source.nameMiddle
            && compareto.nameLast == source.nameLast
            && compareto.contactEmail == source.contactEmail
            && compareto.contactPhone == source.contactPhone
            && compareto.contactPhoneTrip == source.contactPhoneTrip
            && compareto.contactPhoneMobile == source.contactPhoneMobile
            && compareto.contactPhoneOffice == source.contactPhoneOffice
            && compareto.contactFax == source.contactFax
            && compareto.docType == source.docType
            && compareto.docNum == source.docNum
            && compareto.docIssuePlace == source.docIssuePlace
            && compareto.docIssueDate == source.docIssueDate
            && compareto.docExpiryDate == source.docExpiryDate
            && compareto.docVat == source.docVat
            && compareto.docCf == source.docCf
            && compareto.birthDate == source.birthDate
            && compareto.birthPlace == source.birthPlace
            && compareto.birthCountry == source.birthCountry
            && compareto.birthState == source.birthState
            && compareto.locCountry == source.locCountry
            && compareto.locState == source.locState
            && compareto.locCity == source.locCity
            && compareto.locAddress == source.locAddress
            && compareto.locZipCode == source.locZipCode
            && compareto.isActive == source.isActive
            && compareto.createdDate == source.createdDate
            && compareto.createdUserID == source.createdUserID
            && compareto.createdUserNameFull == source.createdUserNameFull
            && compareto.notesInner == source.notesInner
            && compareto.notesInvoice == source.notesInvoice
            && 2 == 2;
    }
    public static bool isComplete(this dbAuthClientTBL source)
    {
        return 1 == 1
            && source.id == source.id
            && source.uid == source.uid
            && source.code == source.code
            && source.typeCode == source.typeCode
            && source.pidLang == source.pidLang
            && source.authUsr == source.authUsr
            && source.authPwd == source.authPwd
            && source.nameHonorific == source.nameHonorific
            && source.nameFull == source.nameFull
            && source.nameFirst == source.nameFirst
            && source.nameMiddle == source.nameMiddle
            && source.nameLast == source.nameLast
            && source.contactEmail == source.contactEmail
            && source.contactPhone == source.contactPhone
            && source.contactPhoneTrip == source.contactPhoneTrip
            && source.contactPhoneMobile == source.contactPhoneMobile
            && source.contactPhoneOffice == source.contactPhoneOffice
            && source.contactFax == source.contactFax
            && source.docType == source.docType
            && source.docNum == source.docNum
            && source.docIssuePlace == source.docIssuePlace
            && source.docIssueDate == source.docIssueDate
            && source.docExpiryDate == source.docExpiryDate
            && source.docVat == source.docVat
            && source.docCf == source.docCf
            && source.birthDate == source.birthDate
            && source.birthPlace == source.birthPlace
            && source.birthCountry == source.birthCountry
            && source.birthState == source.birthState
            && source.locCountry == source.locCountry
            && source.locState == source.locState
            && source.locCity == source.locCity
            && source.locAddress == source.locAddress
            && source.locZipCode == source.locZipCode
            && source.isActive == source.isActive
            && source.createdDate == source.createdDate
            && source.createdUserID == source.createdUserID
            && source.createdUserNameFull == source.createdUserNameFull
            && source.notesInner == source.notesInner
            && source.notesInvoice == source.notesInvoice
            && 2 == 2;
    }
    public static dbAuthClientTypeLK Clone(this dbAuthClientTypeLK source)
    {
        dbAuthClientTypeLK clone = new dbAuthClientTypeLK();
        clone.code = source.code;
        clone.title = source.title;
        clone.isActive = source.isActive;
        return clone;
    }
    #endregion
}
//public class adminBasePage : System.Web.UI.Page
//{
//    #region ModAuth
//    private string tmpPageType;
//    public string PageType
//    {
//        get { if (this.tmpPageType != null)return this.tmpPageType; else return "superadmin"; }
//        set { this.tmpPageType = value; }
//    }
//    public void checkCreate(object sender, EventArgs e)
//    {
//        if (!canCreate)
//            (sender as Control).Visible = false;
//    }
//    public void checkDelete(object sender, EventArgs e)
//    {
//        if (!canDelete)
//            (sender as Control).Visible = false;
//    }
//    public void checkEdit(object sender, EventArgs e)
//    {
//        if (!canEdit)
//            (sender as Control).Visible = false;
//    }
//    public bool canView { get { return authUtils.hasPermission(PageType); } }
//    public bool canCreate { get { return authUtils.hasPermission(PageType, authModes.CanCreate); } }
//    public bool canDelete { get { return authUtils.hasPermission(PageType, authModes.CanDelete); } }
//    public bool canEdit { get { return authUtils.hasPermission(PageType, authModes.CanEdit); } }
//    protected override void OnPreLoad(EventArgs e)
//    {
//        base.OnPreLoad(e);
//        if (!canView)
//        {
//            Response.Redirect("/admin/");
//            Response.End();
//            return;
//        }
//    }
//    #endregion
//    protected override void OnInit(EventArgs e)
//    {
//        base.OnInit(e);
//        CultureInfo objCultureInfo = new CultureInfo("it-IT");
//        objCultureInfo.NumberFormat.NumberDecimalSeparator = ",";
//        objCultureInfo.NumberFormat.NumberGroupSeparator = ".";
//        objCultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
//        objCultureInfo.NumberFormat.CurrencyGroupSeparator = ".";
//        objCultureInfo.NumberFormat.CurrencySymbol = "€";
//        Thread.CurrentThread.CurrentUICulture = objCultureInfo;
//        Thread.CurrentThread.CurrentCulture = objCultureInfo;
//        App.LangID = "it";
//        if (!IsPostBack)
//        {
//            if (authUtils.CurrentUserID == 0 && !Request.Url.AbsoluteUri.Contains("http://-localhost"))
//            {
//                Response.Redirect("/admin/login.aspx?referer=" + Server.UrlEncode(Request.RawUrl));
//            }
//        }
//    }
//    protected void CloseRadWindow(string args)
//    {
//        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseRadWindow", "CloseRadWindow('" + args + "');", true);
//    }
//}