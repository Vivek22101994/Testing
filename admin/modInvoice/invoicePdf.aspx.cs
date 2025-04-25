using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModInvoice.admin.modInvoice
{
    public partial class invoicePdf : System.Web.UI.Page
    {
        public long IdInvoice
        {
            get
            {
                return HF_IdInvoice.Value.ToInt64();
            }
            set
            {
                HF_IdInvoice.Value = value.ToString();
            }
        }
        private dbInvInvoiceTBL TMPcurrInvoice;
        public dbInvInvoiceTBL tblInvoice
        {
            get
            {
                if (TMPcurrInvoice == null)
                    using (DCmodInvoice dc = new DCmodInvoice())
                        TMPcurrInvoice = dc.dbInvInvoiceTBLs.SingleOrDefault(x => x.id == IdInvoice);
                return TMPcurrInvoice ?? new dbInvInvoiceTBL();
            }
        }
        public string currLang
        {
            get
            {
                return HF_pidLang.Value;
            }
            set
            {
                HF_pidLang.Value = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["uid"] != null)
                {
                    Guid _unique = new Guid(Request.QueryString["uid"]);
                    using (DCmodInvoice dc = new DCmodInvoice())
                        TMPcurrInvoice = dc.dbInvInvoiceTBLs.SingleOrDefault(x => x.uid == _unique);
                    if (TMPcurrInvoice != null)
                    {
                        IdInvoice = TMPcurrInvoice.id;
                        currLang = "it";
                        return;
                    }
                }
                Response.Clear();
                Response.End();
                return;
            }
        }
    }
}
