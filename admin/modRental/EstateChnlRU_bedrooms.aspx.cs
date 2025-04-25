using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class EstateChnlRU_bedrooms : adminBasePage
    {
        class FeatureItem
        {
            public string Code { get; set; }
            public int Count { get; set; }
            public FeatureItem(string code, int count)
            {
                Code = code;
                Count = count;
            }
            public FeatureItem(string featuresString)
            {
                if (featuresString.splitStringToList("%").Count == 2)
                {
                    Code = featuresString.splitStringToList("%")[0];
                    Count = featuresString.splitStringToList("%")[1].ToInt32();
                }
                else
                {
                    Code = "";
                    Count = 0;
                }
            }
            public string getString()
            {
                return Code + "%" + Count;
            }
        }
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }
        protected string CurrUid
        {
            get
            {
                return HF_uid.Value;
            }
            set
            {
                HF_uid.Value = value.ToString();
            }
        }
        protected string roomType = "Bedroom";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();

                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ltr_apartment.Text = currEstate.code;
                ucNav.IdEstate = IdEstate;
                drpTypeDataBind(ref drpType);
                fillList();
            }
            else
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setTabs", "setTabs();", true);
        }
        protected void drpTypeDataBind(ref DropDownList drp)
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                drp.DataSource = dcChnl.dbRntChnlRentalsUnitedLkRoomSubTypeTBLs.Where(x => x.type == roomType).OrderBy(x => x.title).ToList();
                drp.DataTextField = "title";
                drp.DataValueField = "code";
                drp.DataBind();
            }
        }
        public void fillList()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var langList = contProps.LangTBL.Where(x => x.is_active == 1).OrderBy(x => x.id).ToList();
                LvLangTab.DataSource = langList;
                LvLangTab.DataBind();
                LvLangPane.DataSource = langList;
                LvLangPane.DataBind();
                LvFeatures.DataSource = dcChnl.dbRntChnlRentalsUnitedLkFeatureValuesTBLs.Where(x => x.type == roomType);
                LvFeatures.DataBind();
                foreach (ListViewDataItem LvFeatures_item in LvFeatures.Items)
                {
                    var lbl_code = LvFeatures_item.FindControl("lbl_code") as Label;
                    var drp_featuresCount = LvFeatures_item.FindControl("drp_featuresCount") as DropDownList;
                    drp_featuresCount.bind_Numbers(1, 10, 1, 0);
                }
                var currList = dcChnl.dbRntChnlRentalsUnitedEstateRoomTBs.Where(x => x.pidEstate == IdEstate && x.roomType == roomType).OrderBy(x => x.roomSubType).ToList();
                if (currList.Count == 0)
                {
                    var currEstate = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate && x.is_active == 1);
                    if (currEstate == null)
                    {
                        Response.Redirect("/admin/rnt_estate_list.aspx");
                        return;
                    }
                    currList = ChnlRentalsUnitedUtils.getRooms(currEstate, roomType);
                    foreach (var currTbl in currList)
                    {
                        dcChnl.Add(currTbl);
                        dcChnl.SaveChanges();
                    }
                }
                Lv.DataSource = currList;
                Lv.DataBind();
                int roomCount = 0;
                foreach (ListViewDataItem item in Lv.Items)
                {
                    roomCount++;
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var this_drpType = item.FindControl("drpType") as DropDownList;
                    var this_LvFeatures = item.FindControl("LvFeatures") as ListView;
                    var this_LvLangTab = item.FindControl("LvLangTab") as ListView;
                    var this_LvLangPane = item.FindControl("LvLangPane") as ListView;

                    var lnkEdit = item.FindControl("lnkEdit") as LinkButton;
                    var lnkSave = item.FindControl("lnkSave") as LinkButton;
                    var lnkCancel = item.FindControl("lnkCancel") as LinkButton;
                    var lnkDel = item.FindControl("lnkDel") as LinkButton;
                    var ltrNumber = item.FindControl("ltrNumber") as Literal;
                    ltrNumber.Text = roomCount + "";

                    var currTbl = currList.SingleOrDefault(x => x.uid.ToString().ToLower() == lbl_id.Text.ToLower());
                    if (currTbl == null)
                    {
                        item.Visible = false;
                        continue;
                    }
                    var featuresList = new List<FeatureItem>();
                    var featuresListString = currTbl.features.splitStringToList("|");
                    foreach (var featuresString in featuresListString)
                        featuresList.Add(new FeatureItem(featuresString));
                    this_LvFeatures.DataSource = dcChnl.dbRntChnlRentalsUnitedLkFeatureValuesTBLs.Where(x => x.type == roomType);
                    this_LvFeatures.DataBind();
                    foreach (ListViewDataItem LvFeatures_item in this_LvFeatures.Items)
                    {
                        var lbl_code = LvFeatures_item.FindControl("lbl_code") as Label;
                        var drp_featuresCount = LvFeatures_item.FindControl("drp_featuresCount") as DropDownList;
                        var chk_featuresSelected = LvFeatures_item.FindControl("chk_featuresSelected") as CheckBox;
                        drp_featuresCount.bind_Numbers(1, 10, 1, 0);
                        var features = featuresList.FirstOrDefault(x => x.Code.ToLower().Trim() == lbl_code.Text.ToLower().Trim());
                        if (features != null)
                        {
                            drp_featuresCount.setSelectedValue(features.Count);
                            chk_featuresSelected.Checked = features.Count > 0;
                        }
                        drp_featuresCount.Enabled = chk_featuresSelected.Enabled = CurrUid.ToLower() == currTbl.uid.ToString().ToLower();
                    }

                    this_LvLangTab.DataSource = langList;
                    this_LvLangTab.DataBind();
                    this_LvLangPane.DataSource = langList;
                    this_LvLangPane.DataBind();
                    foreach (ListViewDataItem langItem in this_LvLangPane.Items)
                    {
                        var langItem_lbl_id = langItem.FindControl("lbl_id") as Label;
                        var langItem_txt_title = langItem.FindControl("txt_title") as TextBox;
                        var langItem_txt_description = langItem.FindControl("txt_description") as TextBox;
                        var currLn = dcChnl.dbRntChnlRentalsUnitedEstateRoomLNs.SingleOrDefault(x => x.pidRoom == currTbl.uid && x.pidLang == langItem_lbl_id.Text);
                        if (currLn != null)
                        {
                            langItem_txt_title.Text = currLn.title;
                            langItem_txt_description.Text = currLn.description;
                        }
                    }
                    drpTypeDataBind(ref this_drpType);
                    this_drpType.setSelectedValue(currTbl.roomSubType);

                    this_drpType.Enabled = CurrUid.ToLower() == currTbl.uid.ToString().ToLower();

                    lnkEdit.Visible = CurrUid.ToLower() != currTbl.uid.ToString().ToLower();
                    lnkSave.Visible = CurrUid.ToLower() == currTbl.uid.ToString().ToLower();
                    lnkCancel.Visible = CurrUid.ToLower() == currTbl.uid.ToString().ToLower();
                }
            }
        }

        protected void LvItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (lbl_id == null) return;
            if (e.CommandName == "del")
            {
                using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
                {
                    var currTbl = dcChnl.dbRntChnlRentalsUnitedEstateRoomTBs.SingleOrDefault(x => x.uid.ToString().ToLower() == lbl_id.Text.ToLower());
                    if (currTbl != null)
                    {
                        dcChnl.Delete(currTbl);
                        dcChnl.SaveChanges();
                    }
                }
                fillList();
            }
            if (e.CommandName == "mod")
            {
                CurrUid = lbl_id.Text.ToLower();
                fillList();
            }
            if (e.CommandName == "canc")
            {
                CurrUid = "";
                fillList();
            }
            if (e.CommandName == "sav")
            {
                var this_drpType = e.Item.FindControl("drpType") as DropDownList;
                var this_LvFeatures = e.Item.FindControl("LvFeatures") as ListView;
                var this_LvLangTab = e.Item.FindControl("LvLangTab") as ListView;
                var this_LvLangPane = e.Item.FindControl("LvLangPane") as ListView;

                var lnkEdit = e.Item.FindControl("lnkEdit") as LinkButton;
                var lnkSave = e.Item.FindControl("lnkSave") as LinkButton;
                var lnkCancel = e.Item.FindControl("lnkCancel") as LinkButton;
                var lnkDel = e.Item.FindControl("lnkDel") as LinkButton;


                using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
                {
                    var currTbl = dcChnl.dbRntChnlRentalsUnitedEstateRoomTBs.SingleOrDefault(x => x.uid.ToString().ToLower() == lbl_id.Text.ToLower());
                    if (currTbl == null)
                    {
                        fillList();
                        return;
                    }
                    var featuresList = new List<FeatureItem>();
                    foreach (ListViewDataItem LvFeatures_item in this_LvFeatures.Items)
                    {
                        var lbl_code = LvFeatures_item.FindControl("lbl_code") as Label;
                        var drp_featuresCount = LvFeatures_item.FindControl("drp_featuresCount") as DropDownList;
                        var chk_featuresSelected = LvFeatures_item.FindControl("chk_featuresSelected") as CheckBox;
                        if (chk_featuresSelected.Checked)
                            featuresList.Add(new FeatureItem(lbl_code.Text, drp_featuresCount.getSelectedValueInt()));
                    }
                    currTbl.features = featuresList.Select(x => x.getString()).ToList().listToString("|");
                    currTbl.roomSubType = this_drpType.SelectedValue;
                    dcChnl.SaveChanges();
                    foreach (ListViewDataItem langItem in this_LvLangPane.Items)
                    {
                        var langItem_lbl_id = langItem.FindControl("lbl_id") as Label;
                        var langItem_txt_title = langItem.FindControl("txt_title") as TextBox;
                        var langItem_txt_description = langItem.FindControl("txt_description") as TextBox;
                        var currLn = dcChnl.dbRntChnlRentalsUnitedEstateRoomLNs.SingleOrDefault(x => x.pidRoom == currTbl.uid && x.pidLang == langItem_lbl_id.Text);
                        if (currLn == null)
                        {
                            currLn = new dbRntChnlRentalsUnitedEstateRoomLN();
                            currLn.pidRoom = currTbl.uid;
                            currLn.pidLang = langItem_lbl_id.Text;
                            dcChnl.Add(currLn);
                        }
                        currLn.title = langItem_txt_title.Text;
                        currLn.description = langItem_txt_description.Text;
                        dcChnl.SaveChanges();
                    }
                }
                CurrUid = "";
                fillList();
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var currTbl = new dbRntChnlRentalsUnitedEstateRoomTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = IdEstate;
                currTbl.roomType = roomType;
                var featuresList = new List<FeatureItem>();
                foreach (ListViewDataItem LvFeatures_item in LvFeatures.Items)
                {
                    var lbl_code = LvFeatures_item.FindControl("lbl_code") as Label;
                    var drp_featuresCount = LvFeatures_item.FindControl("drp_featuresCount") as DropDownList;
                    var chk_featuresSelected = LvFeatures_item.FindControl("chk_featuresSelected") as CheckBox;
                    if (chk_featuresSelected.Checked)
                        featuresList.Add(new FeatureItem(lbl_code.Text, drp_featuresCount.getSelectedValueInt()));
                }
                currTbl.features = featuresList.Select(x => x.getString()).ToList().listToString("|");
                currTbl.roomSubType = drpType.SelectedValue;
                dcChnl.Add(currTbl);
                dcChnl.SaveChanges();
                foreach (ListViewDataItem langItem in LvLangPane.Items)
                {
                    var langItem_lbl_id = langItem.FindControl("lbl_id") as Label;
                    var langItem_txt_title = langItem.FindControl("txt_title") as TextBox;
                    var langItem_txt_description = langItem.FindControl("txt_description") as TextBox;
                    var currLn = dcChnl.dbRntChnlRentalsUnitedEstateRoomLNs.SingleOrDefault(x => x.pidRoom == currTbl.uid && x.pidLang == langItem_lbl_id.Text);
                    if (currLn == null)
                    {
                        currLn = new dbRntChnlRentalsUnitedEstateRoomLN();
                        currLn.pidRoom = currTbl.uid;
                        currLn.pidLang = langItem_lbl_id.Text;
                        dcChnl.Add(currLn);
                    }
                    currLn.title = langItem_txt_title.Text;
                    currLn.description = langItem_txt_description.Text;
                    dcChnl.SaveChanges();
                }
            }
            CurrUid = "";
            fillList();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
        }

    }
}