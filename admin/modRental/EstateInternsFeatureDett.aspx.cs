using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateInternsFeatureDett : adminBasePage
    {
        protected dbRntEstateInternsFeatureTB currTBL;
        private List<dbRntEstateInternsFeatureLN> TMPcurrLangs;
        private List<dbRntEstateInternsFeatureLN> currLangs
        {
            get
            {
                if (TMPcurrLangs == null)
                    if (ViewState["currLangs"] != null)
                    {
                        TMPcurrLangs =
                            PConv.DeserArrToList((object[])ViewState["currLangs"],
                                                 typeof(dbRntEstateInternsFeatureLN)).Cast<dbRntEstateInternsFeatureLN>().ToList();
                    }
                    else
                        TMPcurrLangs = new List<dbRntEstateInternsFeatureLN>();

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
            if (!IsPostBack)
            {
                HfId.Value = Request.QueryString["id"].ToInt64().ToString();
                drp_isActive.DataBind();
                drp_pidInternsType_DataBind();
                fillData();
            }
        }

        private void drp_pidInternsType_DataBind()
        {
            drp_pidInternsType.Items.Insert(0, new ListItem("- - -", ""));
            drp_pidInternsType.Items.Insert(1, new ListItem("Bedroom"));
            drp_pidInternsType.Items.Insert(2, new ListItem(contUtils.getLabel("lblBathRooms", App.LangID, "Bathroom")));
            drp_pidInternsType.Items.Insert(3, new ListItem(contUtils.getLabel("lblKitchenType", App.LangID, "Kitchen")));
            drp_pidInternsType.Items.Insert(4, new ListItem("Livingroom"));
        }

        private void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateInternsFeatureTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    CloseRadWindow("reload");
                    currTBL = new dbRntEstateInternsFeatureTB();
                    return;
                }
                ltrTitle.Text = contUtils.getLabel("lblInternFeature", App.LangID, "Caratteristiche") + currTBL.code + " / id=" + currTBL.id;
                txt_code.Text = currTBL.code;
                drp_isActive.setSelectedValue(currTBL.isActive);
                drp_pidInternsType.setSelectedValue(currTBL.pidInternsType);
                currLangs = dc.dbRntEstateInternsFeatureLNs.Where(x => x.pidInternsFeature == currTBL.id).ToList();
                HfLang.Value = App.DefLangID + "";
                FillLang();
                BindLvLangs();
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntEstateInternsFeatureTBs.SingleOrDefault(x => x.id == HfId.Value.ToInt32());
                if (currTBL == null)
                {
                    currTBL = new dbRntEstateInternsFeatureTB();
                    dc.Add(currTBL);
                }
                currTBL.code = txt_code.Text;
                currTBL.isActive = drp_isActive.getSelectedValueInt(0);
                currTBL.pidInternsType = drp_pidInternsType.SelectedValue;
                //   if (UserAuthentication.CurrRoleTBL.viewOnlyNoSave.objToInt32() == 0) { dc.SaveChanges(); }
                dc.SaveChanges();
                SaveAllLangs(currTBL.id);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"" + contUtils.getLabel("lblSuccess", App.LangID, "Le modifiche sono state salvate con successo") + ".\", 340, 110);", true);
            }
            using (DCmodRental dc = new DCmodRental())
                rntProps.EstateInternsSubTypeVIEW = dc.dbRntEstateInternsSubTypeVIEWs.ToList();
        }

        protected void lnkNew_Click(object sender, EventArgs e)
        {
            HfId.Value = "0";
            fillData();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
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
            lnk.CssClass = HfLang.Value == lbl_id.Text ? "tab_item_current lang_" + lbl_id.Text : "tab_item lang_" + lbl_id.Text;
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
            var rlLang = currLangsTmp.SingleOrDefault(x => x.pidLang == HfLang.Value.objToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntEstateInternsFeatureLN();
                rlLang.pidInternsFeature = HfId.Value.ToInt32();
                rlLang.pidLang = HfLang.Value.objToInt32();
                currLangsTmp.Add(rlLang);
            }
            rlLang.description = re_description.Content;
            rlLang.title = txt_title.Text;
            currLangs = currLangsTmp;
        }
        protected void FillLang()
        {
            var rlLang = currLangs.SingleOrDefault(x => x.pidLang == HfLang.Value.objToInt32());
            if (rlLang == null)
            {
                rlLang = new dbRntEstateInternsFeatureLN();
            }

            txt_title.Text = rlLang.title;
            re_description.Content = rlLang.description;
        }
        protected void SaveAllLangs(int id)
        {
            SaveLang();
            using (DCmodRental dc = new DCmodRental())
            {
                foreach (var rl in currLangs)
                {
                    if (dc.dbRntEstateInternsFeatureLNs.SingleOrDefault(x => x.pidInternsFeature == id && x.pidLang == rl.pidLang) == null)
                    {
                        rl.pidInternsFeature = id;
                        dc.Add(rl);
                    }
                    else
                    {
                        var currLN = dc.dbRntEstateInternsFeatureLNs.Single(x => x.pidInternsFeature == id && x.pidLang == rl.pidLang);
                        rl.CopyTo(ref currLN);
                    }
                }
                // if (UserAuthentication.CurrRoleTBL.viewOnlyNoSave.objToInt32() == 0) {  }
                dc.SaveChanges();
            }
        }
    }
}

