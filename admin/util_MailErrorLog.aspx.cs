using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace RentalInRome.admin
{
    public partial class util_MailErrorLog : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] _files = Directory.GetFiles(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_folder.Value));
                List<string> _data = new List<string>();
                _data.AddRange(_files.ToList());
                _data = _data.OrderByDescending(x => x).ToList();
                drp_logList.Items.Clear();
                drp_logList.Items.Add(new ListItem("-seleziona-", ""));
                foreach (string _f in _data)
                {
                    FileInfo fi = new FileInfo(_f);
                    drp_logList.Items.Add(new ListItem("" + fi.Name.Replace(".xml",""), "" + fi.Name));
                }

                Fill_LV();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            Fill_LV();
        }
        protected void Fill_LV()
        {
            string _fileName = drp_logList.SelectedValue;
            if (_fileName == "")
            {
                LV.DataSource = new List<mailUtils.MailLogItem>();
                LV.DataBind();
                return;
            }
            mailUtils.MailLogList _list = new mailUtils.MailLogList(_fileName);
            if (txt_IP.Text.Trim() != "")
                _list.Items = _list.Items.Where(x => x.IP.Contains(txt_IP.Text.Trim())).ToList();
            if (txt_Subject.Text.Trim() != "")
                _list.Items = _list.Items.Where(x => x.Subject.Contains(txt_Subject.Text.Trim())).ToList();
            LV.DataSource = _list.Items.OrderByDescending(x => x.Date);
            LV.DataBind();
            lnk_resendAll.Visible = _list.Items.FirstOrDefault(x => x.isResent == false) != null;
        }
        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            Label lbl_id = LV.Items[e.NewSelectedIndex].FindControl("lbl_id") as Label;
            if (lbl_id != null)
            {
            }
            Fill_LV();
            LV.SelectedIndex = e.NewSelectedIndex;
        }
        protected void drp_logList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void lnk_flt_Click(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void lnk_resendAll_Click(object sender, EventArgs e)
        {
            string _fileName = drp_logList.SelectedValue;
            if (_fileName == "")
            {
                return;
            }
            int toStop = 0;
            mailUtils.MailLogList _list = new mailUtils.MailLogList(_fileName);
            foreach (var item in _list.Items)
            {
                if (item.isResent) continue;
                if (MailingUtilities.customSendMailTo(item.Subject, item.Body, item.TOs, item.CCs, item.Attachments, item.bcc, item.FROM_MAIL, item.FROM_NAME, "reinvio: " + item.mailDescrtiption))
                {
                    item.isResent = true; 
                    _list.Save();
                }
                else
                    toStop++;
                if (toStop == 2)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Errore nel reinvio mail,<br/>è stato creato un'altro log.\", 340, 110);", true);
                    mailUtils._list = null;
                    return;
                }
            }
            mailUtils._list = null;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Tutte le mail sono state reinviate con successo.\", 340, 110);", true);
        }

    }
}
