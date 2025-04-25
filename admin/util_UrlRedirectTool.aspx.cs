using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin
{
    public partial class util_UrlRedirectTool : adminBasePage
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
        protected void Fill_LV()
        {
            RedirectList _redirectList = new RedirectList();
            LV.DataSource = _redirectList.Items.OrderBy(x => x.From);
            LV.DataBind();
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
                Label lbl_name = e.Item.FindControl("lbl_name") as Label;
                TextBox txt_from = e.Item.FindControl("txt_from") as TextBox;
                TextBox txt_to = e.Item.FindControl("txt_to") as TextBox;
                RedirectList _redirectList = new RedirectList();
                RedirectItem _item = _redirectList.Items.FirstOrDefault(x => x.From == lbl_name.Text);
                if (_item != null)
                {
                    _item.From = txt_from.Text.Trim().ToLower();
                    _item.To = txt_to.Text.Trim().ToLower();
                    _redirectList.Save();
                    UrlRedirectTool.CURRENT_TOOL = _redirectList;
                    LV.SelectedIndex = -1;
                    Fill_LV();
                }
            }
            if (e.CommandName == "elimina")
            {
                Label lbl_name = e.Item.FindControl("lbl_name") as Label;
                RedirectList _redirectList = new RedirectList();
                RedirectItem _item = _redirectList.Items.FirstOrDefault(x => x.From == lbl_name.Text);
                if (_item != null)
                {
                    _redirectList.Items.Remove(_item);
                    _redirectList.Save();
                    UrlRedirectTool.CURRENT_TOOL = _redirectList;
                    LV.SelectedIndex = -1;
                    Fill_LV();
                }
            }
        }

        protected void lnk_new_Click(object sender, EventArgs e)
        {
            RedirectList _redirectList = new RedirectList();
            RedirectItem _item = new RedirectItem();
            _item.From = "  // da cambiare nuovo";
            _item.To = "  // da cambiare nuovo";
            _redirectList.Items.Add(_item);
            _redirectList.Save();
            LV.SelectedIndex = 0;
            Fill_LV();
        }
    }
}
