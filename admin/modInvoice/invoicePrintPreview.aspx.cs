using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModAuth;

namespace ModInvoice.admin.modInvoice
{
    public partial class invoicePrintPreview : System.Web.UI.Page
    {
        public class tmpOwnerClass
        {
            public long id { get; set; }
            public string nameFull { get; set; }
            public tmpOwnerClass(long Id, string NameFull)
            {
                id = Id;
                nameFull = NameFull;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setFilters();
            }
        }
        protected void setFilters()
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            using (DCmodAuth dcAuth = new DCmodAuth())
            {
                ltrFilter.Text = "";
                string sep = "";
                List<dbInvInvoiceTBL> list = dc.dbInvInvoiceTBLs.ToList();
                if (!string.IsNullOrEmpty(Request.QueryString["docNum"]))
                {
                    ltrFilter.Text += sep + "Num. Fattura: " + Request.QueryString["docNum"];
                    sep = " - ";
                    list = list.Where(x => x.docNum != null && x.docNum.ToLower().Contains(Request.QueryString["docNum"].urlDecode().ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["cashInOut"]))
                {
                    ltrFilter.Text += sep + (Request.QueryString["cashInOut"].ToInt32() == 1 ? "Fatture Attive" : "Fatture Passive");
                    sep = " - ";
                    list = list.Where(x => x.cashInOut == Request.QueryString["cashInOut"].ToInt32()).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["cashDateFrom"]))
                {
                    ltrFilter.Text += sep + "dal: " + Request.QueryString["cashDateFrom"].JSCal_stringToDate().formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----");
                    sep = " - ";
                    list = list.Where(x => x.docIssueDate >= Request.QueryString["cashDateFrom"].JSCal_stringToDate()).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["cashDateTo"]))
                {
                    ltrFilter.Text += sep + "al: " + Request.QueryString["cashDateTo"].JSCal_stringToDate().formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----") + "(escluso)";
                    sep = " - ";
                    list = list.Where(x => x.docIssueDate < Request.QueryString["cashDateTo"].JSCal_stringToDate()).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["ownerType"]))
                {
                    ltrFilter.Text += sep + "Tutti " + Request.QueryString["ownerType"];
                    sep = " - ";
                    list = list.Where(x => x.ownerType != null && x.ownerType.ToLower().Contains(Request.QueryString["ownerType"].urlDecode().ToLower().Trim())).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["ownerIdsList"]))
                {
                    ltrFilter.Text += sep + "con esclusioni ";
                    sep = " - ";
                    List<long?> ownerIdsList = Request.QueryString["ownerIdsList"].splitStringToList("|").Select(x => x.ToInt64() as long?).ToList();
                    list = list.Where(x => !ownerIdsList.Contains(x.ownerId)).ToList();
                }
                List<tmpOwnerClass> ownerList = new List<tmpOwnerClass>();
                List<long> ownerIdList = list.Select(x => x.ownerId.objToInt64()).Distinct().ToList();
                foreach(long tmpID in ownerIdList)
                {
                    dbAuthClientTBL ownerTBL = dcAuth.dbAuthClientTBLs.SingleOrDefault(x => x.id == tmpID);
                    if (ownerTBL != null)
                    {
                        ownerList.Add(new tmpOwnerClass(tmpID, ownerTBL.nameFull));
                    }
                    else
                    {
                        ownerList.Add(new tmpOwnerClass(tmpID, list.First(x => x.ownerId == tmpID).ownerNameFull+"*"));
                    }
                }
                LVowner.DataSource = ownerList.OrderBy(x => x.nameFull);
                LVowner.DataBind();
                foreach (ListViewDataItem item in LVowner.Items)
                {
                    ListView LVlist = item.FindControl("LVlist") as ListView;
                    Label lbl_id = item.FindControl("lbl_id") as Label;
                    if (LVlist == null || lbl_id == null) continue;
                    List<dbInvInvoiceTBL> TMPlist = list.Where(x => x.ownerId == lbl_id.Text.ToInt64()).ToList();
                    LVlist.DataSource = TMPlist.OrderBy(x => x.docNum).ToList();
                    LVlist.DataBind();
                    Literal TMPltr_cashTaxFree = LVlist.FindControl("ltr_cashTaxFree") as Literal;
                    Literal TMPltr_cashTaxAmount = LVlist.FindControl("ltr_cashTaxAmount") as Literal;
                    Literal TMPltr_cashTotalAmount = LVlist.FindControl("ltr_cashTotalAmount") as Literal;
                    if (TMPltr_cashTaxFree == null || TMPltr_cashTaxAmount == null || TMPltr_cashTotalAmount == null) continue;
                    TMPltr_cashTaxFree.Text = TMPlist.Sum(x => x.cashTaxFree.Value).ToString("N2");
                    TMPltr_cashTaxAmount.Text = TMPlist.Sum(x => x.cashTaxAmount.Value).ToString("N2");
                    TMPltr_cashTotalAmount.Text = TMPlist.Sum(x => x.cashTotalAmount.Value).ToString("N2");
                }

                ltr_cashTaxFree.Text = list.Sum(x => x.cashTaxFree.Value).ToString("N2");
                ltr_cashTaxAmount.Text = list.Sum(x => x.cashTaxAmount.Value).ToString("N2");
                ltr_cashTotalAmount.Text = list.Sum(x => x.cashTotalAmount.Value).ToString("N2");
            }
        }
    }

}