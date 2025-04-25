using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental.ucEstateDett
{
    public partial class EstateInterns : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE currEstate;
        public string pidInternsType
        {
            get
            {
                return HF_Type.Value;
            }
            set
            {
                HF_Type.Value = value;
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
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                currEstate = DC_RENTAL.RNT_TB_ESTATE.FirstOrDefault(x => x.id == IdEstate);
                setTypeLabel();
                drpInternsSubTypeDataBind(ref drpSubType);
                fillList();
            }
        }

        public int CurrId
        {
            get
            {
                return HF_CurrId.Value.ToInt32();
            }
            set
            {
                HF_CurrId.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
            }
        }

        protected void drpInternsSubTypeDataBind(ref DropDownList drp)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                drp.DataSource = dc.dbRntEstateInternsSubTypeVIEWs.Where(x => x.pidInternsType == pidInternsType && x.pidLang == App.LangID && x.isActive == 1).OrderBy(x => x.title).ToList();
                drp.DataTextField = "title";
                drp.DataValueField = "id";
                drp.DataBind();
                drp.Items.Insert(0, new ListItem("- - -", ""));
            }
        }

        protected void setTypeLabel()
        {
            if (pidInternsType == "Bedroom")
                lblInternType.Text = contUtils.getLabel("lblBedRooms");
            else if (pidInternsType == "Bathroom")
                lblInternType.Text = contUtils.getLabel("lblBathRooms");
            else if (pidInternsType == "Kitchen")
                lblInternType.Text = contUtils.getLabel("lblKitchenType");
            else if (pidInternsType == "Livingroom")
                lblInternType.Text = contUtils.getLabel("lblLivingRoom");
        }

        protected int getNumberOfInterns()
        {
            int num = 0;
            if (pidInternsType == "Bedroom")
            {
                num = currEstate.num_rooms_bed.objToInt32();
            }
            else if (pidInternsType == "Bathroom")
            {
                num = currEstate.num_rooms_bath.objToInt32();
            }
            else if (pidInternsType == "Kitchen")
            {
                num = currEstate.num_kitchen.objToInt32();
            }
            else if (pidInternsType == "Livingroom")
            {
                num = currEstate.num_rooms_living.objToInt32();
            }
            return num;
        }

        protected void ManageNumberOfInterns(int cnt, int InternSubTypeId)
        {
            //if (pidInternsType == "Bedroom")
            //{
            //    if (InternSubTypeId != 4)
            //        return;
            //}
            //currEstate = DC_RENTAL.RNT_TB_ESTATE.FirstOrDefault(x => x.id == IdEstate);
            //if (pidInternsType == "Bedroom")
            //    currEstate.num_rooms_bed = currEstate.num_rooms_bed + cnt;
            //else if (pidInternsType == "Bathroom")
            //    currEstate.num_rooms_bath = currEstate.num_rooms_bath + cnt;
            //else if (pidInternsType == "Kitchen")
            //    currEstate.num_kitchen = currEstate.num_kitchen + cnt;
            //else if (pidInternsType == "Livingroom")
            //    currEstate.num_rooms_living = currEstate.num_rooms_living + cnt;

            //if (pidInternsType == "Bedroom" || pidInternsType == "Kitchen" || pidInternsType == "Livingroom")
            //    currEstate.num_rooms_total = currEstate.num_rooms_total + cnt;
            //DC_RENTAL.SubmitChanges();

            currEstate = DC_RENTAL.RNT_TB_ESTATE.FirstOrDefault(x => x.id == IdEstate);
            if (currEstate == null) return;
            using (DCmodRental DC = new DCmodRental())
            {
                var Interns = DC.dbRntEstateInternsTBs.Where(x => x.pidEstate == IdEstate).ToList();
                int num_rooms_beds = 0;
                int num_rooms_bath = 0;
                int num_kitchen = 0;
                int num_rooms_living = 0;
                foreach (dbRntEstateInternsTB intern in Interns)
                {
                    if (intern.pidInternsType == "Bedroom")
                        num_rooms_beds = num_rooms_beds + 1;

                    else if (intern.pidInternsType == "Bathroom")
                        num_rooms_bath = num_rooms_bath + 1;

                    else if (intern.pidInternsType == "Kitchen")
                        num_kitchen = num_kitchen + 1;
                    else if (intern.pidInternsType == "Livingroom")
                        num_rooms_living = num_rooms_living + 1;
                }

                currEstate.num_rooms_bed = num_rooms_beds;
                currEstate.num_rooms_bath = num_rooms_bath;
                currEstate.num_kitchen = num_kitchen;
                currEstate.num_rooms_living = num_rooms_living;
                currEstate.num_rooms_total = num_rooms_beds + num_rooms_living + num_kitchen;
                DC_RENTAL.SubmitChanges();
                AppSettings._refreshCache_RNT_ESTATEs();
                AppSettings.RELOAD_SESSION();
            }
        }

        public void fillList()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var langList = contProps.LangTBL.Where(x => x.is_active == 1).ToList();
                var lstFeatures = dc.dbRntEstateInternsFeatureVIEWs.Where(x => x.pidInternsType == pidInternsType && x.pidLang == App.LangID && x.isActive == 1).ToList();
                if (lstFeatures == null)
                    lstFeatures = new List<dbRntEstateInternsFeatureVIEW>();

                #region For Aggiungi(New)
                LvLangTab.DataSource = langList;
                LvLangTab.DataBind();
                LvLangPane.DataSource = langList;
                LvLangPane.DataBind();

                LvFeatures.DataSource = lstFeatures;
                LvFeatures.DataBind();
                foreach (ListViewDataItem LvFeatures_item in LvFeatures.Items)
                {
                    var drp_featuresCount = LvFeatures_item.FindControl("drp_featuresCount") as DropDownList;
                    drp_featuresCount.bind_Numbers(1, 10, 1, 0);
                }
                #endregion

                var currEstateInternsList = dc.dbRntEstateInternsTBs.Where(x => x.pidEstate == IdEstate && x.pidInternsType == pidInternsType).ToList();
                #region Add Interns if count 0
                if (currEstateInternsList.Count == 0)
                {
                    int num = getNumberOfInterns();

                    //else if (pidInternsType == "Kitchen")
                    //    num = currEstate.num_.objToInt32();

                    if (num > 0)
                    {
                        var defaultInternsSubType = dc.dbRntEstateInternsSubTypeVIEWs.FirstOrDefault(x => x.pidLang == App.LangID);

                        for (int i = 1; i <= num; i++)
                        {
                            var currEstIntern = new dbRntEstateInternsTB();
                            currEstIntern.pidEstate = IdEstate;
                            currEstIntern.pidInternsType = pidInternsType;
                            currEstIntern.pidInternsSubType = defaultInternsSubType != null ? defaultInternsSubType.id : 0;
                            dc.Add(currEstIntern);
                            dc.SaveChanges();
                        }
                        currEstateInternsList = dc.dbRntEstateInternsTBs.Where(x => x.pidEstate == IdEstate && x.pidInternsType == pidInternsType).ToList();
                    }
                }
                #endregion
                Lv.DataSource = currEstateInternsList;
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

                    // var lnkEdit = item.FindControl("lnkEdit") as LinkButton;
                    //  var lnkSave = item.FindControl("lnkSave") as LinkButton;
                    // var lnkCancel = item.FindControl("lnkCancel") as LinkButton;
                    var lnkDel = item.FindControl("lnkDel") as LinkButton;
                    var ltrNumber = item.FindControl("ltrNumber") as Literal;
                    ltrNumber.Text = roomCount + "";

                    var currTbl = currEstateInternsList.FirstOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTbl == null)
                    {
                        item.Visible = false;
                        continue;
                    }


                    //Interns SubType
                    drpInternsSubTypeDataBind(ref this_drpType);
                    this_drpType.setSelectedValue(currTbl.pidInternsSubType);
                    //  this_drpType.Enabled = CurrId == currTbl.id;

                    //Interns Features
                    this_LvFeatures.DataSource = lstFeatures;
                    this_LvFeatures.DataBind();

                    foreach (ListViewDataItem LvFeatures_item in this_LvFeatures.Items)
                    {
                        var lbl_featureid = LvFeatures_item.FindControl("lbl_featureid") as Label;

                        var drp_featuresCount = LvFeatures_item.FindControl("drp_featuresCount") as DropDownList;
                        drp_featuresCount.bind_Numbers(1, 10, 1, 0);

                        var chk_featuresSelected = LvFeatures_item.FindControl("chk_featuresSelected") as CheckBox;

                        var estateFeaturesRL = dc.dbRntEstateInternsFeatureRLs.FirstOrDefault(x => x.pidEstateInterns == currTbl.id && x.pidInternsFeature == lbl_featureid.Text.ToInt32());
                        if (estateFeaturesRL != null)
                        {
                            drp_featuresCount.setSelectedValue(estateFeaturesRL.count);
                            chk_featuresSelected.Checked = estateFeaturesRL.count > 0;
                        }
                        //  drp_featuresCount.Enabled = chk_featuresSelected.Enabled = CurrId == currTbl.id;
                    }


                    //Language Data
                    this_LvLangTab.DataSource = langList;
                    this_LvLangTab.DataBind();
                    this_LvLangPane.DataSource = langList;
                    this_LvLangPane.DataBind();
                    foreach (ListViewDataItem langItem in this_LvLangPane.Items)
                    {
                        var langItem_lbl_id = langItem.FindControl("lbl_id") as Label;
                        var langItem_txt_title = langItem.FindControl("txt_title") as TextBox;
                        var langItem_txt_description = langItem.FindControl("txt_description") as TextBox;
                        var currLn = dc.dbRntEstateInternsLNs.FirstOrDefault(x => x.pidEstateInterns == currTbl.id && x.pidLang == langItem_lbl_id.Text.objToInt32());
                        if (currLn != null)
                        {
                            langItem_txt_title.Text = currLn.title;
                            langItem_txt_description.Text = currLn.description;
                        }
                    }

                    //lnkEdit.Visible = CurrId != currTbl.id;
                    //lnkSave.Visible = CurrId == currTbl.id;
                    //lnkCancel.Visible = CurrId == currTbl.id;
                }
                if (roomCount == 0)
                    divSaveAll.Visible = false;
                else
                    divSaveAll.Visible = true;
            }
        }

        protected void LvItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (lbl_id == null) return;
            if (e.CommandName == "del")
            {
                int currSubType = 0;
                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var currTbl = dc.RntEstateInternsTB.FirstOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    if (currTbl != null)
                    {
                        var currLnList = dc.RntEstateInternsLN.Where(x => x.pidEstateInterns == lbl_id.Text.ToInt32()).ToList();
                        if (currLnList != null && currLnList.Count() > 0)
                            dc.RntEstateInternsLN.DeleteAllOnSubmit(currLnList);

                        currSubType = currTbl.pidInternsSubType;
                        dc.RntEstateInternsTB.DeleteOnSubmit(currTbl);
                        dc.SubmitChanges();

                        ManageNumberOfInterns(-1, currSubType);
                    }
                }
                fillList();
            }
            //if (e.CommandName == "mod")
            //{
            //    CurrId = lbl_id.Text.ToInt32();
            //    fillList();
            //}
            //if (e.CommandName == "canc")
            //{
            //    CurrId = 0;
            //    fillList();
            //}


        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                var currTbl = new RntEstateInternsTB();
                currTbl.pidEstate = IdEstate;
                currTbl.pidInternsType = pidInternsType;
                currTbl.pidInternsSubType = drpSubType.getSelectedValueInt();
                dc.RntEstateInternsTB.InsertOnSubmit(currTbl);
                dc.SubmitChanges();

                foreach (ListViewDataItem LvFeatures_item in LvFeatures.Items)
                {
                    var lbl_featureid = LvFeatures_item.FindControl("lbl_featureid") as Label;
                    var drp_featuresCount = LvFeatures_item.FindControl("drp_featuresCount") as DropDownList;
                    var chk_featuresSelected = LvFeatures_item.FindControl("chk_featuresSelected") as CheckBox;
                    if (chk_featuresSelected.Checked)
                    {
                        var estateFeaturesRL = new RntEstateInternsFeatureRL();
                        estateFeaturesRL.pidEstateInterns = currTbl.id;
                        estateFeaturesRL.pidInternsFeature = lbl_featureid.Text.ToInt32();
                        estateFeaturesRL.count = drp_featuresCount.getSelectedValueInt();
                        dc.RntEstateInternsFeatureRL.InsertOnSubmit(estateFeaturesRL);
                        dc.SubmitChanges();
                    }
                }

                foreach (ListViewDataItem langItem in LvLangPane.Items)
                {
                    var langItem_lbl_id = langItem.FindControl("lbl_id") as Label;
                    var langItem_txt_title = langItem.FindControl("txt_title") as TextBox;
                    var langItem_txt_description = langItem.FindControl("txt_description") as TextBox;
                    var currLn = dc.RntEstateInternsLN.FirstOrDefault(x => x.pidEstateInterns == currTbl.id && x.pidLang == langItem_lbl_id.Text.objToInt32());
                    if (currLn == null)
                    {
                        currLn = new RntEstateInternsLN();
                        currLn.pidEstateInterns = currTbl.id;
                        currLn.pidLang = langItem_lbl_id.Text.objToInt32();
                        dc.RntEstateInternsLN.InsertOnSubmit(currLn);
                    }
                    currLn.title = langItem_txt_title.Text;
                    currLn.description = langItem_txt_description.Text;
                    dc.SubmitChanges();
                }

                ManageNumberOfInterns(1, currTbl.pidInternsSubType);
            }
            CurrId = 0;

            fillList();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillList();
        }

        protected void lnk_save_all_Click(object sender, EventArgs e)
        {

            foreach (ListViewDataItem item in Lv.Items)
            {
                var this_lbl = item.FindControl("lbl_id") as Label;
                var this_drpType = item.FindControl("drpType") as DropDownList;
                var this_LvFeatures = item.FindControl("LvFeatures") as ListView;
                var this_LvLangTab = item.FindControl("LvLangTab") as ListView;
                var this_LvLangPane = item.FindControl("LvLangPane") as ListView;

                //var lnkEdit = item.FindControl("lnkEdit") as LinkButton;
                //var lnkSave = item.FindControl("lnkSave") as LinkButton;
                //var lnkCancel = item.FindControl("lnkCancel") as LinkButton;
                var lnkDel = item.FindControl("lnkDel") as LinkButton;


                using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
                {
                    var currTbl = dc.RntEstateInternsTB.FirstOrDefault(x => x.id == this_lbl.Text.ToInt32());
                    if (currTbl == null)
                    {
                        fillList();
                        return;
                    }

                    var estateFeaturesRLList = dc.RntEstateInternsFeatureRL.Where(x => x.pidEstateInterns == currTbl.id).ToList();
                    if (estateFeaturesRLList != null && estateFeaturesRLList.Count() > 0)
                    {
                        dc.RntEstateInternsFeatureRL.DeleteAllOnSubmit(estateFeaturesRLList);
                        dc.SubmitChanges();
                    }

                    foreach (ListViewDataItem LvFeatures_item in this_LvFeatures.Items)
                    {
                        var lbl_featureid = LvFeatures_item.FindControl("lbl_featureid") as Label;
                        var drp_featuresCount = LvFeatures_item.FindControl("drp_featuresCount") as DropDownList;
                        var chk_featuresSelected = LvFeatures_item.FindControl("chk_featuresSelected") as CheckBox;
                        if (chk_featuresSelected.Checked)
                        {
                            var estateFeaturesRL = new RntEstateInternsFeatureRL();
                            estateFeaturesRL.pidEstateInterns = currTbl.id;
                            estateFeaturesRL.pidInternsFeature = lbl_featureid.Text.ToInt32();
                            estateFeaturesRL.count = drp_featuresCount.getSelectedValueInt();
                            dc.RntEstateInternsFeatureRL.InsertOnSubmit(estateFeaturesRL);
                            dc.SubmitChanges();
                        }
                    }

                    currTbl.pidInternsSubType = this_drpType.getSelectedValueInt();
                    dc.SubmitChanges();

                    foreach (ListViewDataItem langItem in this_LvLangPane.Items)
                    {
                        var langItem_lbl_id = langItem.FindControl("lbl_id") as Label;
                        var langItem_txt_title = langItem.FindControl("txt_title") as TextBox;
                        var langItem_txt_description = langItem.FindControl("txt_description") as TextBox;
                        var currLn = dc.RntEstateInternsLN.FirstOrDefault(x => x.pidEstateInterns == currTbl.id && x.pidLang == langItem_lbl_id.Text.objToInt32());
                        if (currLn == null)
                        {
                            currLn = new RntEstateInternsLN();
                            currLn.pidEstateInterns = currTbl.id;
                            currLn.pidLang = langItem_lbl_id.Text.objToInt32();
                            dc.RntEstateInternsLN.InsertOnSubmit(currLn);
                        }
                        currLn.title = langItem_txt_title.Text;
                        currLn.description = langItem_txt_description.Text;
                        dc.SubmitChanges();
                    }
                }
                CurrId = 0;


                fillList();
            }
        }
    }
}