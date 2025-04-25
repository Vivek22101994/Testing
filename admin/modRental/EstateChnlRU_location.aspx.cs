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
    public partial class EstateChnlRU_location : adminBasePage
    {
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
                ltr_apartment.Text = currEstate.code + " / " + "rif. " + IdEstate;
                UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                fillData();
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";            
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
            rlLang.location_other_activities = re_other_activties.Content;
            rlLang.location_description = re_description.Content;
            CURRENT_LANG = curr_rl_langs;
        }

        protected void Fill_lang(string pid_lang)
        {
            var rlLang = CURRENT_LANG.SingleOrDefault(item => item.pid_lang == pid_lang);
            if (rlLang == null)
            {
                rlLang = new dbRntChnlRentalsUnitedEstateLN();
            }

            re_description.Content = rlLang.location_description;
            re_other_activties.Content = rlLang.location_other_activities;

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
                        curr_rl.location_description = rl.location_description;
                        curr_rl.location_other_activities = rl.location_other_activities;



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
            fillData();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }
        protected void fillData()
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                var currTbl = dcChnl.dbRntChnlRentalsUnitedEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                drp_default_zoom_level.bind_Numbers(0, 25, 1, 0);
                drp_max_zoom_level.bind_Numbers(0, 25, 1, 0);
                txt_address1.Text = currEstate.loc_address;
                txt_address2.Text = currEstate.loc_inner_bell;
                txt_postal_code.Text = currEstate.loc_zip_code;
                txt_city.Text = CurrentSource.loc_cityTitle(currEstate.pid_city.objToInt32(), App.DefLangID, "");
                drp_allow_zoom.setSelectedValue(currTbl.allow_traveler_zoom);
                drp_default_zoom_level.setSelectedValue(currTbl.default_zoom_level);
                drp_max_zoom_level.setSelectedValue(currTbl.max_zoom_level);
                drp_show_exact_location.setSelectedValue(currTbl.show_exact_location);
                if (currEstate.google_maps != "" && currEstate.google_maps != null)
                {
                    if (currEstate.google_maps.Split('|').Length > 1)
                    {
                        txt_latitude.Text = currEstate.google_maps.Split('|')[0];
                        txt_longitude.Text = currEstate.google_maps.Split('|')[1];
                    }
                    else if (currEstate.google_maps.Split(',').Length > 1)
                    {
                        txt_latitude.Text = currEstate.google_maps.Split(',')[0].Replace(".", ",");
                        txt_longitude.Text = currEstate.google_maps.Split(',')[1].Replace(".", ",");
                    }
                }
                CURRENT_LANG = dcChnl.dbRntChnlRentalsUnitedEstateLNs.Where(x => x.pid_estate == currTbl.id).ToList();
                HF_lang.Value = "ita";
                Fill_lang(HF_lang.Value);
                LV_langs.DataBind();

                //fill neatest places

                List<LOC_VIEW_POINT> lstPoints = maga_DataContext.DC_LOCATION.LOC_VIEW_POINT.Where(x => x.haPlaceType != null && x.haPlaceType != "0").ToList();
                if (lstPoints != null && lstPoints.Count > 0)
                {
                    LV_nearestPlaces.DataSource = lstPoints;
                    LV_nearestPlaces.DataBind();
                }
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
                currTbl.allow_traveler_zoom = drp_allow_zoom.getSelectedValueInt(0);
                currTbl.default_zoom_level = drp_default_zoom_level.getSelectedValueInt(0);
                currTbl.max_zoom_level = drp_max_zoom_level.getSelectedValueInt(0);
                currTbl.show_exact_location = drp_show_exact_location.getSelectedValueInt(0);
                //for nearest places
                foreach (ListViewDataItem objItem in LV_nearestPlaces.Items)
                {
                    Label lbl_id = objItem.FindControl("lbl_id") as Label;
                    DropDownList drp_distance = objItem.FindControl("drp_distance") as DropDownList;
                    DropDownList drp_unit = objItem.FindControl("drp_unit") as DropDownList;

                    var currPoint = dcChnl.dbRntChnlRentalsUnitedEstatePointsRLs.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_point == lbl_id.Text.objToInt32());
                    if (drp_distance.getSelectedValueInt() != -1)
                    {
                        if (currPoint != null)
                        {
                            currPoint.distance = drp_distance.getSelectedValueInt().ToString();
                            currPoint.unit = drp_unit.SelectedValue;
                        }
                        else
                        {
                            currPoint = new dbRntChnlRentalsUnitedEstatePointsRL();
                            currPoint.pid_estate = IdEstate;
                            currPoint.pid_point = lbl_id.Text.objToInt32();
                            currPoint.distance = drp_distance.getSelectedValueInt().ToString();
                            currPoint.unit = drp_unit.SelectedValue;
                            dcChnl.Add(currPoint);
                        }
                    }
                    else
                    {
                        if (currPoint != null)
                        {
                            dcChnl.Delete(currPoint);
                        }
                    }
                }
                dcChnl.SaveChanges(); 
                Save_RL_langs();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate correttamente.\", 340, 110);", true);
                fillData();
            }
        }
        protected void LV_nearestPlaces_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            using (DCchnlRentalsUnited dcChnl = new DCchnlRentalsUnited())
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                    DropDownList drp_distance = e.Item.FindControl("drp_distance") as DropDownList;
                    DropDownList drp_unit = e.Item.FindControl("drp_unit") as DropDownList;
                    for (int i = 0; i <= 100; i++)
                    {
                        if (i % 5 == 0)
                        {
                            drp_distance.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        }
                    }
                    drp_distance.Items.Insert(0, new ListItem("-seleziona-", "-1"));

                    //fill data on load
                    var currPoint = dcChnl.dbRntChnlRentalsUnitedEstatePointsRLs.SingleOrDefault(x => x.pid_estate == IdEstate && x.pid_point == lbl_id.Text.objToInt32());
                    if (currPoint != null)
                    {
                        drp_distance.setSelectedValue(currPoint.distance);
                        drp_unit.setSelectedValue(currPoint.unit);
                    }


                }
            }
        }
    }
}