using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ModInvoice.admin.modInvoice
{
    public partial class cashBookPrintPreview : System.Web.UI.Page
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
            {
                ltrFilter.Text = "";
                string sep = "";
                List<dbInvCashBookVIEW> list = dc.dbInvCashBookVIEWs.ToList();
                if (!string.IsNullOrEmpty(Request.QueryString["cashInOut"]))
                {
                    ltrFilter.Text += sep + (Request.QueryString["cashInOut"].ToInt32() == 1 ? "Pagamenti in Entrata" : "Pagamenti in Uscita");
                    sep = " - ";
                    list = list.Where(x => x.cashInOut == Request.QueryString["cashInOut"].ToInt32()).ToList();
                }
                else
                {
                    ltrFilter.Text += sep + "Tutti Pagamenti ";
                    sep = " - ";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["cashDateFrom"]))
                {
                    ltrFilter.Text += sep + "dal: " + Request.QueryString["cashDateFrom"].JSCal_stringToDate().formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----");
                    sep = " - ";
                    list = list.Where(x => x.cashDate >= Request.QueryString["cashDateFrom"].JSCal_stringToDate()).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["cashDateTo"]))
                {
                    ltrFilter.Text += sep + "al: " + Request.QueryString["cashDateTo"].JSCal_stringToDate().formatCustom("#dd#/#mm#/#yy#", 1, "--/--/----") + "(escluso)";
                    sep = " - ";
                    list = list.Where(x => x.cashDate < Request.QueryString["cashDateTo"].JSCal_stringToDate()).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["cashPlaceList"]))
                {
                    list = list.Where(x => !Request.QueryString["cashPlaceList"].splitStringToList("|").Select(y => y.ToInt32() as int?).Contains(x.cashPlace)).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["cashTypeList"]))
                {
                    list = list.Where(x => !Request.QueryString["cashTypeList"].splitStringToList("|").Select(y => y.ToInt32() as int?).Contains(x.cashType)).ToList();
                }
                if (!string.IsNullOrEmpty(Request.QueryString["docCaseCodeIdList"]))
                {
                    list = list.Where(x => !Request.QueryString["docCaseCodeIdList"].splitStringToList("|").Select(y => y.ToInt32() as int?).Contains(x.docCaseId)).ToList();
                }
                LVlist.DataSource = list.OrderBy(x => x.cashDate).ToList();
                LVlist.DataBind();

                int count_1 = list.Where(x => x.cashInOut == 1).Count();
                decimal cashAmount_1 = list.Where(x => x.cashInOut == 1).Sum(x => x.cashAmount.objToDecimal());
                ltr_count_1.Text = count_1.ToString();
                ltr_cashAmount_1.Text = cashAmount_1.ToString("N2");

                int count_0 = list.Where(x => x.cashInOut == 0).Count();
                decimal cashAmount_0 = list.Where(x => x.cashInOut == 0).Sum(x => x.cashAmount.objToDecimal());
                ltr_count_0.Text = count_0.ToString();
                ltr_cashAmount_0.Text = cashAmount_0.ToString("N2");

                ltr_count_diff.Text = (count_1 + count_0).ToString();
                ltr_cashAmount_diff.Text = (cashAmount_1 - cashAmount_0).ToString("N2");
            }
        }
    }

}