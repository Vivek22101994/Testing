using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class util_sendMail : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "util_sendMail";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                chkList_flt_pidLang.DataSource = contProps.LangTBL.Where(x => x.is_active == 1);
                chkList_flt_pidLang.DataTextField = "title";
                chkList_flt_pidLang.DataValueField = "id";
                chkList_flt_pidLang.DataBind();
                chkList_agency_DataBind();
                re_body.Content = ltr_template_1.Text;
            }
        }
        protected void chkList_agency_DataBind()
        {
            chkList_agency.Items.Clear();
            var langIds = chkList_flt_pidLang.getSelectedValueList().Select(x=>(int?)x.ToInt32()).ToList();
            var tmpList = rntProps.AgentTBL.Where(x => x.isActive == 1 && !string.IsNullOrEmpty(x.contactEmail) && langIds.Contains(x.pidLang)).OrderBy(x => x.nameCompany);
            foreach (var tmp in tmpList)
            {
                chkList_agency.Items.Add(new ListItem("" + tmp.nameCompany + " - " + tmp.nameFull, "" + tmp.contactEmail));
            }
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            chkList_agency_DataBind();
        }
        protected void lnk_send_Click(object sender, EventArgs e)
        {
            List<string> Tos = chkList_agency.getSelectedValueList();
            if (Tos.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert(\"Selezionare almeno un destinatario.\");", true);
                return;
            }
            MailingUtilities.SendMailToMany(txt_subject.Text, re_body.Content, Tos, new List<string>(), false, txt_fromMail.Text, txt_fromName.Text, "util_sendMail", true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert(\"L'invio è stato iniziato, potrebbe richiedere diversi minuti per terminare.\");", true);
        }
        protected void lnk_chkListSelectAll_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (sender as LinkButton);
            string arg = lnk.CommandArgument;
            if (arg.Contains("pidLang"))
                foreach (ListItem item in chkList_flt_pidLang.Items)
                    item.Selected = !arg.Contains("deselect");
            if (arg.Contains("agency"))
                foreach (ListItem item in chkList_agency.Items)
                    item.Selected = !arg.Contains("deselect");
        }

    }
}
