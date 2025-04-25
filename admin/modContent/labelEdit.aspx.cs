using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModContent.admin.modContent
{
    public partial class labelEdit : adminBasePage
    {
        private List<string> listTypes = new List<string>() { "sys", "menu", "form" };
        private List<dbContLabelTBL> TMPcurrLangs;
        private List<dbContLabelTBL> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbContLabelTBL)).Cast<dbContLabelTBL>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbContLabelTBL>();

                return TMPcurrLangs;
            }
            set
            {
                ViewState["currLangs"] = PConv.SerialList(value.Cast<object>().ToList());
                TMPcurrLangs = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                drp_flt_type.Items.Add(new ListItem("-tutti-", ""));
                foreach (string type in listTypes)
                { 
                    drp_type.Items.Add(type);
                    drp_flt_type.Items.Add(type);
                }
                drp_flt_pidLang.DataSource = contProps.LangTBL.Where(x => x.id != 1).OrderBy(x => x.id);
                drp_flt_pidLang.DataTextField = "title";
                drp_flt_pidLang.DataValueField = "id";
                drp_flt_pidLang.DataBind();
                drp_flt_pidLang.Items.Add(new ListItem("-tutti-", ""));
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "dett")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                HfId.Value = lbl_id.Text;
                fillData();
            }
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodContent dc = new DCmodContent())
                {
                    List<dbContLabelTBL> list = dc.dbContLabelTBLs.Where(x => x.id == lbl_id.Text).ToList();
                    foreach (dbContLabelTBL currTBL in list)
                    {
                        dc.Delete(currTBL);
                    }
                    dc.SaveChanges();
                    contProps.LabelTBL = dc.dbContLabelTBLs.ToList();
                    contProps.LabelVIEW = dc.dbContLabelVIEWs.ToList();
                }
                closeDetails(false);
            }
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            LV_DataBind(true);
        }
        protected void LV_DataBind(bool filter)
        {
            List<dbContLabelVIEW> currList = new List<dbContLabelVIEW>();
            if (drp_flt_type.SelectedValue != "" || txt_flt_id.Text != "" || txt_flt_mTitle.Text != "" || txt_flt_title.Text != "")
            {
                contProps.LabelVIEW = null;
                currList = contProps.LabelVIEW;
                if (drp_flt_pidLang.SelectedValue != "")
                    currList = currList.Where(x => x.pidLang == drp_flt_pidLang.SelectedValue.ToInt32()).ToList();
                if (drp_flt_type.SelectedValue != "")
                    currList = currList.Where(x => x.type == drp_flt_type.SelectedValue).ToList();
                if (txt_flt_id.Text != "")
                    currList = currList.Where(x => x.id.ToLower().Trim().Contains(txt_flt_id.Text.ToLower().Trim())).ToList();
                if (txt_flt_mTitle.Text != "")
                    currList = currList.Where(x => x.mTitle.ToLower().Trim().Contains(txt_flt_mTitle.Text.ToLower().Trim())).ToList();
                if (txt_flt_title.Text != "")
                    currList = currList.Where(x => x.title.ToLower().Trim().Contains(txt_flt_title.Text.ToLower().Trim())).ToList();
                if (drp_flt_withNoValue.SelectedValue == "main")
                    currList = currList.Where(x => string.IsNullOrEmpty(x.mTitle)).ToList();
                if (drp_flt_withNoValue.SelectedValue == "other")
                    currList = currList.Where(x => string.IsNullOrEmpty(x.title)).ToList();
                if (drp_flt_withNoValue.SelectedValue == "all")
                    currList = currList.Where(x => string.IsNullOrEmpty(x.mTitle) || string.IsNullOrEmpty(x.title)).ToList();
            }
            else
            {
                LV.Visible = false;
                if (filter)
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Seleziona almeno un parametro di ricerca.\", 340, 110);", true);
            }
            LV.DataSource = currList.OrderBy(x => x.id).ThenBy(x => x.pidLang);
            LV.DataBind();

        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            LV_DataBind(false);
            rwdDett.VisibleOnPageLoad = false;
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        private void fillData()
        {
            using (DCmodContent dc = new DCmodContent())
            {
                dbContLabelTBL currTBL = dc.dbContLabelTBLs.FirstOrDefault(x => x.id == HfId.Value);
                if (currTBL == null)
                {
                    ltrTitle.Text = "Inserisci nuova variabile";
                    txt_id.Text = "";
                }
                else
                {
                    ltrTitle.Text = "Cambia variabile: " + currTBL.id;
                    txt_id.Text = currTBL.id;
                    drp_type.setSelectedValue(currTBL.type);
                }

                rwdDett.VisibleOnPageLoad = true;
                currLangs = dc.dbContLabelTBLs.Where(x => x.id == HfId.Value).ToList();
                HfLang.Value = "1";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            using (DCmodContent dc = new DCmodContent())
            {
                if(txt_id.Text.Trim()=="")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Il Nome della variabile deve avere un valore.\", 340, 110);", true);
                    return;
                }
                dbContLabelTBL currTBL = dc.dbContLabelTBLs.FirstOrDefault(x => x.id != HfId.Value && x.id == txt_id.Text);
                if (currTBL != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Nel database essite già una variabile con nome '" + txt_id.Text + "'.<br/>Inserisci un nome diverso oppure cambia quella esistente.\", 340, 110);", true);
                    return;
                }
                SaveLang();
                List<dbContLabelTBL> list = dc.dbContLabelTBLs.Where(x => x.id == HfId.Value).ToList();
                foreach (dbContLabelTBL tmpTBL in list)
                {
                    dc.Delete(tmpTBL);
                }
                dc.SaveChanges();
                foreach (var rl in currLangs)
                {
                    rl.id = txt_id.Text;
                    rl.type = drp_type.SelectedValue;
                    dc.Add(rl);
                }
                dc.SaveChanges();
                contProps.LabelTBL = dc.dbContLabelTBLs.ToList();
                contProps.LabelVIEW = dc.dbContLabelVIEWs.ToList();
            }
            closeDetails(true);
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected void BindLvLangs()
        {
            LvLangs.DataSource = contProps.LangTBL.Where(x => x.is_active == 1).OrderBy(x => x.id);
            LvLangs.DataBind();
        }
        protected void LvLangs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkLang");
            lnk.CssClass = HfLang.Value == lbl_id.Text ? "tab_item_current" : "tab_item";
        }
        protected void LvLangs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                SaveLang();
                HfLang.Value = lbl_id.Text;
                FillLang();
                BindLvLangs();
            }
        }
        protected void SaveLang()
        {
            var currLangsTmp = currLangs;
            var rlLang = currLangsTmp.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbContLabelTBL();
                rlLang.id = HfId.Value;
                rlLang.pidLang = HfLang.Value.ToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.title = txt_title.Text;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.ToInt32());
            if (rlLang == null)
            {
                rlLang = new dbContLabelTBL();
            }
            txt_title.Text = rlLang.title;
        }
    }

}