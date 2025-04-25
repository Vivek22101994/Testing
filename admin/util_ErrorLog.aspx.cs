using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class util_ErrorLog : adminBasePage
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
        protected void Fill_LV()
        {
            string _fileName = drp_logList.SelectedValue;
            if (_fileName == "")
            {
                LV.DataSource = new List<LogItem>();
                LV.DataBind();
                return;
            }
            LogList _list = new LogList(_fileName);
            if (txt_IP.Text.Trim() != "")
                _list.Items = _list.Items.Where(x => x.IP.Contains(txt_IP.Text.Trim())).ToList();
            if (txt_Url.Text.Trim() != "")
                _list.Items = _list.Items.Where(x => x.Url.Contains(txt_Url.Text.Trim())).ToList();
            if (txt_Value.Text.Trim() != "")
                _list.Items = _list.Items.Where(x => x.Value.Contains(txt_Value.Text.Trim())).ToList();
            LV.DataSource = _list.Items.OrderByDescending(x => x.Date);
            LV.DataBind();
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

    }
}
