using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.admin.modRental
{
    public partial class EstateChnlRU_unit : adminBasePage
    {
        public class MsgClass
        {
            public string Msg { get; set; }
            public string Type { get; set; }
            public MsgClass(string msg, string type)
            {
                Msg = msg;
                Type = type;
            }
        }
        private static string LastMsgSessionKey = "EstateChnlRU_unit_LastMsg";
        protected MsgClass LastMsg
        {
            get
            {
                if (HttpContext.Current.Session[LastMsgSessionKey] != null)
                {
                    return (MsgClass)HttpContext.Current.Session[LastMsgSessionKey];
                }
                return new MsgClass("", "");
            }
            set
            {
                HttpContext.Current.Session[LastMsgSessionKey] = value;
            }
        }
        protected string featureType = "Unit";
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

                if (Request.QueryString["updatefrommagarental"] == "true")
                {
                    ChnlRentalsUnitedUtils.updateEstateFromMagarental(currEstate);
                    LastMsg = new MsgClass("<b>Successfully updated</b>", "mainline alert-ok");
                    Response.Redirect("EstateChnlRU_unit.aspx?id=" + IdEstate);
                    return;
                }
                ltr_apartment.Text = currEstate.code + " / " + "rif. " + IdEstate;
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                FillControls();
                if (LastMsg.Msg != "")
                {
                    pnlError.Visible = true;
                    pnlError.Attributes["class"] = LastMsg.Type;
                    ltrErorr.Text = LastMsg.Msg;
                    LastMsg = new MsgClass("", "");
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected dbRntChnlRentalsUnitedEstateFeaturesRL currFeature;
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
        private List<dbRntChnlRentalsUnitedEstateLN> CURRENT_LANG_;
        private List<dbRntChnlRentalsUnitedEstateLN> CURRENT_LANG
        {
            get
            {
                if (CURRENT_LANG_ == null)
                    if (ViewState["CURRENT_LANG"] != null)
                    {
                        CURRENT_LANG_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LANG"],
                                                 typeof(dbRntChnlRentalsUnitedEstateLN)).Cast<dbRntChnlRentalsUnitedEstateLN>().ToList();
                    }
                    else
                        CURRENT_LANG_ = new List<dbRntChnlRentalsUnitedEstateLN>();

                return CURRENT_LANG_;
            }
            set
            {
                ViewState["CURRENT_LANG"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_LANG_ = value;
            }
        }
        protected void LV_langs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk_lang");
            lnk.CssClass = HF_lang.Value == lbl_id.Text ? "tab_item_current lang_" + lbl_id.Text : "tab_item lang_" + lbl_id.Text;
        }

        protected void LV_langs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "change_lang")
            {
                Label lbl_id = (Label)e.Item.FindControl("lbl_id");
                Save_lang();
                HF_lang.Value = lbl_id.Text;
                Fill_lang(HF_lang.Value);
                LV_langs.DataBind();
            }
        }
        protected void Save_lang()
        {
            var curr_rl_langs = CURRENT_LANG;
            var rlLang = curr_rl_langs.SingleOrDefault(item => item.pid_lang == HF_lang.Value);
            if (rlLang == null)
            {
                rlLang = new dbRntChnlRentalsUnitedEstateLN();
                rlLang.pid_estate = IdEstate;
                rlLang.pid_lang = HF_lang.Value;
                CURRENT_LANG.Add(rlLang);

            }
            rlLang.bedroom_details = re_bedroom_details.Content;
            rlLang.bathroom_details = re_bathroom_details.Content;
            rlLang.features_description = re_features_description.Content;
            rlLang.unit_description = re_unit_description.Content;
            rlLang.unit_name = txt_unit_name.Text;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang(string pid_lang)
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == pid_lang);
            if (rlLang == null)
            {
                rlLang = new dbRntChnlRentalsUnitedEstateLN();
            }

            re_unit_description.Content = rlLang.unit_description;
            re_bedroom_details.Content = rlLang.bedroom_details;
            re_bathroom_details.Content = rlLang.bathroom_details;
            re_features_description.Content = rlLang.features_description;
            txt_unit_name.Text = rlLang.unit_name;

        }

        protected void Save_RL_langs()
        {
            Save_lang();
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var curr_rl_langs = dcChnl.dbRntChnlRentalsUnitedEstateLNs.Where(x => x.pid_estate == IdEstate).ToList();
                foreach (var rl in CURRENT_LANG)
                {
                    if (!curr_rl_langs.Exists(x => x.pid_lang == rl.pid_lang))
                    {
                        rl.pid_estate = IdEstate;
                        dcChnl.Add(rl);
                    }
                    else
                    {
                        var curr_rl = curr_rl_langs.Single(x => x.pid_lang == rl.pid_lang);
                        curr_rl.unit_name = rl.unit_name;
                        curr_rl.unit_description = rl.unit_description;
                        curr_rl.bedroom_details = rl.bedroom_details;
                        curr_rl.bathroom_details = rl.bathroom_details;
                        curr_rl.features_description = rl.features_description;

                    }

                }
                dcChnl.SaveChanges();
                curr_rl_langs = dcChnl.dbRntChnlRentalsUnitedEstateLNs.Where(x => x.pid_estate == IdEstate).ToList();
                foreach (var _lang in contProps.LangTBL.Where(x => x.is_active == 1))
                {
                    var code = contUtils.getLang_code(_lang.id);
                    if (!curr_rl_langs.Exists(x => x.pid_lang == code))
                    {
                        dbRntChnlRentalsUnitedEstateLN _newRL = new dbRntChnlRentalsUnitedEstateLN();
                        _newRL.pid_lang = code;
                        _newRL.pid_estate = IdEstate;
                        //_newRL.title = txt_code.Text;
                        dcChnl.Add(_newRL);
                    }
                }
                dcChnl.SaveChanges();
            }
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        protected void FillControls()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var currTbl = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                drp_lounge_seating.bind_Numbers(0, 100, 1, 0);
                drp_dining_seating.bind_Numbers(0, 100, 1, 0);
                drp_max_sleep.bind_Numbers(0, 100, 1, 0);
                drp_max_sleep_bed.bind_Numbers(0, 100, 1, 0);

                //for master/slave

                Bind_drp_master_estate();
                if (currTbl.is_slave == 1)
                {
                    chk_slave.Checked = true;
                    drp_master_estate.Enabled = true;
                    drp_master_estate.setSelectedValue(currTbl.pid_master_estate);

                }
                else
                {
                    chk_slave.Checked = false;
                    drp_master_estate.Enabled = false;
                }


                //fill property type
                Bind_drp_propertType();
                fillFeatures();
                drp_is_active.setSelectedValue(currTbl.is_active);
                drp_lounge_seating.setSelectedValue(currTbl.num_lounge_seating);
                drp_dining_seating.setSelectedValue(currTbl.num_dining_seating);
                drp_max_sleep.setSelectedValue(currTbl.num_max_sleep);
                drp_max_sleep_bed.setSelectedValue(currTbl.num_max_sleep_bed);
                drp_property_type.setSelectedValue(currTbl.propertyType);
                txt_area.Text = currTbl.mq_inner.objToInt32().ToString();
                drp_unit.setSelectedValue(currTbl.mq_inner_unit);
                CURRENT_LANG = dcChnl.dbRntChnlRentalsUnitedEstateLNs.Where(x => x.pid_estate == currTbl.id).ToList();
                HF_lang.Value = "ita";
                Fill_lang(HF_lang.Value);
                LV_langs.DataBind();

                tdAlert.Visible = string.IsNullOrEmpty(currTbl.propertyType) || currTbl.propertyType == "0";
            }
        }
        protected void fillFeatures()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                var lstFeatures = dcChnl.dbRntChnlRentalsUnitedLkFeatureValuesTBLs.Where(x => x.type == featureType);

                dl_accomodation.DataSource = lstFeatures.Where(x => x.subType == "ACCOMMODATIONS").ToList();
                dl_accomodation.DataBind();

                dl_amenities.DataSource = lstFeatures.Where(x => x.subType == "AMENITIES").ToList();
                dl_amenities.DataBind();

                dl_entertaiment.DataSource = lstFeatures.Where(x => x.subType == "ENTERTAINMENT").ToList();
                dl_entertaiment.DataBind();

                dl_kitchen.DataSource = lstFeatures.Where(x => x.subType == "KITCHEN_DINING").ToList();
                dl_kitchen.DataBind();

                dl_outdoor.DataSource = lstFeatures.Where(x => x.subType == "OUTDOOR").ToList();
                dl_outdoor.DataBind();

                dl_pool.DataSource = lstFeatures.Where(x => x.subType == "POOL_SPA").ToList();
                dl_pool.DataBind();

                dl_suitability.DataSource = lstFeatures.Where(x => x.subType == "SUITABILITY").ToList();
                dl_suitability.DataBind();

                dl_theme.DataSource = lstFeatures.Where(x => x.subType == "THEMES").ToList();
                dl_theme.DataBind();

            }
        }

        protected void saveData(DataListItem item, DCchnlRentalsUnited dcChnl)
        {
            Label lbl_id = item.FindControl("lbl_id") as Label;
            CheckBox chk = item.FindControl("chk") as CheckBox;
            DropDownList drp_count = item.FindControl("drp_count") as DropDownList;
            if (chk.Checked)
            {
                var currRl = dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.type == featureType && x.code == lbl_id.Text);
                if (currRl == null)
                {
                    currRl = new dbRntChnlRentalsUnitedEstateFeaturesRL();
                    currRl.pidEstate = IdEstate;
                    currRl.type = featureType;
                    currRl.code = lbl_id.Text;
                    dcChnl.Add(currRl);
                }
                currRl.count = drp_count.SelectedValue.objToInt32();
                dcChnl.SaveChanges();
            }
        }
        protected void FillDataFromControls()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {

                var currTbl = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }

                currTbl.mq_inner = txt_area.Text.ToInt32();
                currTbl.mq_inner_unit = drp_unit.SelectedValue;
                currTbl.is_active = drp_is_active.getSelectedValueInt();
                currTbl.propertyType = drp_property_type.SelectedValue;
                currTbl.num_lounge_seating = drp_lounge_seating.getSelectedValueInt();
                currTbl.num_dining_seating = drp_dining_seating.getSelectedValueInt();
                currTbl.num_max_sleep = drp_max_sleep.getSelectedValueInt();
                currTbl.num_max_sleep_bed = drp_max_sleep_bed.getSelectedValueInt();

                //for master/slave
                //master/slave
                if (chk_slave.Checked == true)
                {
                    currTbl.is_slave = 1;
                    currTbl.pid_master_estate = drp_master_estate.getSelectedValueInt(0);
                }
                else
                {
                    currTbl.is_slave = 0;
                }
                dcChnl.SaveChanges();

                dcChnl.Delete(dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.Where(x => x.pidEstate == IdEstate && x.type == featureType));
                dcChnl.SaveChanges();
                foreach (DataListItem item in dl_accomodation.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_amenities.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_entertaiment.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_kitchen.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_outdoor.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_pool.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_suitability.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_theme.Items)
                    saveData(item, dcChnl);

                Save_RL_langs();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate correttamente.\", 340, 110);", true);
                FillControls();
            }
        }
        private void Bind_drp_propertType()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                drp_property_type.DataSource = dcChnl.dbRntChnlRentalsUnitedLkPropertyTypeTBLs.OrderBy(x => x.code).ToList();
                drp_property_type.DataTextField = "title";
                drp_property_type.DataValueField = "code";
                drp_property_type.DataBind();
                drp_property_type.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
        }
        protected void dl_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                CheckBox chk = e.Item.FindControl("chk") as CheckBox;
                DropDownList drp_count = e.Item.FindControl("drp_count") as DropDownList;
                var currRl = dcChnl.dbRntChnlRentalsUnitedEstateFeaturesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.type == featureType && x.code == lbl_id.Text);
                if (currRl != null)
                {
                    chk.Checked = true;
                    drp_count.setSelectedValue(currRl.count);
                }
            }
        }
        private void Bind_drp_master_estate()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                List<int> estateIds = dcChnl.dbRntChnlRentalsUnitedEstateTBs.Where(x => x.is_active == 1 && (x.is_slave == 0 || x.is_slave == null) && x.id != IdEstate).Select(x => x.id).ToList();
                var list = AppSettings.RNT_TB_ESTATE.Where(x => estateIds.Contains(x.id)).ToList();
                drp_master_estate.Items.Clear();
                foreach (var t in list)
                {
                    drp_master_estate.Items.Add(new ListItem(t.code, "" + t.id));
                }
                drp_master_estate.Items.Insert(0, new ListItem("-seleziona-", "0"));
            }
        }


    }
}