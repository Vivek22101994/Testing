using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using RentalInRome.data;

namespace ModRental.admin.modRental
{
    public partial class ChnlAtraveoFeatureValuesList : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
                {
                    var list = dcChnl.dbRntChnlAtraveoLkFeatureValuesTBLs.ToList();
                    var values = list.Select(x => x.type).Distinct().ToList();
                    drp_flt_type.Items.Clear();
                    drp_flt_type.Items.Add("");
                    foreach (var value in values)
                        drp_flt_type.Items.Add(value);
                }
                fillData();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            fillData();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "get")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                
            }
        }
        protected void lnk_flt_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected Dictionary<int, string> LocalIds;
        private void fillData()
        {
            if (LocalIds == null)
            {
                LocalIds = new Dictionary<int, string>();
                var LocalIdsList = DC_RENTAL.RNT_TB_CONFIGs.Where(x => 1 == 1).ToList();
                foreach (var tmp in LocalIdsList)
                {
                    var tmpLn = DC_RENTAL.RNT_LN_CONFIGs.SingleOrDefault(x => x.pid_config == tmp.id && x.pid_lang == App.LangID);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_RENTAL.RNT_LN_CONFIGs.SingleOrDefault(x => x.pid_config == tmp.id && x.pid_lang == App.DefLangID);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_RENTAL.RNT_LN_CONFIGs.SingleOrDefault(x => x.pid_config == tmp.id && x.pid_lang == 2);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = DC_RENTAL.RNT_LN_CONFIGs.SingleOrDefault(x => x.pid_config == tmp.id && x.pid_lang == 1);
                    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title))
                        continue;
                    LocalIds.Add(tmp.id, tmpLn.title);
                }

                //LocalIds = new Dictionary<int, string>();
                //var LocalIdsList = rntProps.EstateExtrasTB.Where(x => 1 == 1).ToList();
                //foreach (var tmp in LocalIdsList)
                //{
                //    var tmpLn = rntProps.EstateExtrasVIEW.SingleOrDefault(x => x.id == tmp.id && x.pidLang == App.LangID);
                //    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = rntProps.EstateExtrasVIEW.SingleOrDefault(x => x.id == tmp.id && x.pidLang == App.DefLangID);
                //    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = rntProps.EstateExtrasVIEW.SingleOrDefault(x => x.id == tmp.id && x.pidLang == "en");
                //    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title)) tmpLn = rntProps.EstateExtrasVIEW.SingleOrDefault(x => x.id == tmp.id && x.pidLang == "it");
                //    if (tmpLn == null || string.IsNullOrEmpty(tmpLn.title))
                //        continue;
                //    LocalIds.Add(tmp.id, tmpLn.title);
                //}
            }
            using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            using (DCmodRental dc = new DCmodRental())
            {
                var list = dcChnl.dbRntChnlAtraveoLkFeatureValuesTBLs.ToList();
                if (drp_flt_type.SelectedValue != "") list = list.Where(x => x.type != null && x.type == drp_flt_type.SelectedValue).ToList();
                if (txt_flt_code.Text.Trim() != "") list = list.Where(x => x.code != null && x.code.ToLower().Trim().Contains(txt_flt_code.Text.ToLower().Trim())).ToList();
                if (txt_flt_title.Text.Trim() != "") list = list.Where(x => x.title != null && x.title.ToLower().Trim().Contains(txt_flt_title.Text.ToLower().Trim())).ToList();
                LV.DataSource = list.OrderBy(x => x.type).ThenBy(x => x.code).ToList();
                LV.DataBind();
                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_type = item.FindControl("lbl_type") as Label;
                    Label lbl_code = item.FindControl("lbl_code") as Label;
                    Label lbl_refId = item.FindControl("lbl_refId") as Label;
                    Label lbl_title = item.FindControl("lbl_title") as Label;
                    TextBox txt_title = item.FindControl("txt_title") as TextBox;
                    DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

                    drp_refId.Items.Add(new ListItem("- - -", ""));
                    var extraList = LocalIds.OrderBy(x => x.Value).ToList();
                    foreach (var extra in extraList)
                    {
                        drp_refId.Items.Add(new ListItem(extra.Value, "" + extra.Key));
                    }
                    drp_refId.setSelectedValue(lbl_refId.Text);
                    txt_title.Text = lbl_title.Text;
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {
                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_type = item.FindControl("lbl_type") as Label;
                    Label lbl_code = item.FindControl("lbl_code") as Label;
                    Label lbl_refId = item.FindControl("lbl_refId") as Label;
                    TextBox txt_title = item.FindControl("txt_title") as TextBox;
                    DropDownList drp_refId = item.FindControl("drp_refId") as DropDownList;

                    var featureValuesTbl = dcChnl.dbRntChnlAtraveoLkFeatureValuesTBLs.SingleOrDefault(x => x.code == lbl_code.Text);
                    if (featureValuesTbl != null)
                    {
                        featureValuesTbl.title = txt_title.Text != "" ? txt_title.Text : lbl_code.Text;
                        featureValuesTbl.refId = drp_refId.SelectedValue;
                        dcChnl.SaveChanges();
                    }
                }
            }
            fillData();
        }
    }

}