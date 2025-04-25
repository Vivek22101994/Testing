using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModAppServerCommon
{
    public partial class BlockedIpList : adminBasePage
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
                Fill_LV();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            Fill_LV();
        }
        protected void Fill_LV()
        {
            using (DCmodAppServerCommon dc = new DCmodAppServerCommon())
            {
                LV.DataSource = dc.dbUtlsBlockedIpLSTs.OrderBy(x => x.ip);
                LV.DataBind();
            }
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            LV.SelectedIndex = e.NewSelectedIndex;
            Fill_LV();
        }

        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "salva")
            {
                Label lbl_uid = e.Item.FindControl("lbl_uid") as Label;
                TextBox txt_ip = e.Item.FindControl("txt_ip") as TextBox;
                Guid uid;
                if(lbl_uid==null || txt_ip==null|| !Guid.TryParse(lbl_uid.Text, out uid)) return;
                using (DCmodAppServerCommon dc = new DCmodAppServerCommon())
                {
                    var tmp = dc.dbUtlsBlockedIpLSTs.SingleOrDefault(x => x.uid != uid && x.ip == txt_ip.Text);
                    if (tmp != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Attenzione!<br/>Ip esistente.\", 340, 110);", true);
                        return;
                    }
                    var item = dc.dbUtlsBlockedIpLSTs.SingleOrDefault(x => x.uid == uid);
                    if (item == null) return;
                    item.ip = txt_ip.Text;
                    dc.SaveChanges();
                    LV.SelectedIndex = -1;
                    Fill_LV();
                    BlockedIpTool.CurrList = dc.dbUtlsBlockedIpLSTs.ToList();
                }
            }
            if (e.CommandName == "elimina")
            {
                Label lbl_uid = e.Item.FindControl("lbl_uid") as Label;
                Guid uid;
                if (lbl_uid == null || !Guid.TryParse(lbl_uid.Text, out uid)) return;
                using (DCmodAppServerCommon dc = new DCmodAppServerCommon())
                {
                    var item = dc.dbUtlsBlockedIpLSTs.SingleOrDefault(x => x.uid == uid);
                    if (item == null) return;
                    dc.Delete(item);
                    dc.SaveChanges();
                    LV.SelectedIndex = -1;
                    Fill_LV();
                    BlockedIpTool.CurrList = dc.dbUtlsBlockedIpLSTs.ToList();
                }
            }
        }

    }
}
