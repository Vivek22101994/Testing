using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental
{
    public partial class EstateChnlHA_listing : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected string featureType = "Listing";
       
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
        protected void Page_Load(object sender, EventArgs e)
        {

             DC_RENTAL = maga_DataContext.DC_RENTAL;
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

        private void fillData()
        {
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            {
                var lstListing = dcChnl.dbRntChnlHomeAwayLkFeatureValuesTBLs.Where(x => x.type == featureType);

                dl_attractios.DataSource = lstListing.Where(x => x.subType == "ATTRACTIONS").ToList();
                dl_attractios.DataBind();

                dl_car.DataSource = lstListing.Where(x => x.subType == "CAR").ToList();
                dl_car.DataBind();

                dl_leisure.DataSource = lstListing.Where(x => x.subType == "LEISURE").ToList();
                dl_leisure.DataBind();

                dl_local_features.DataSource = lstListing.Where(x => x.subType == "LOCAL").ToList();
                dl_local_features.DataBind();

                dl_location_type.DataSource = lstListing.Where(x => x.subType == "LOCATION").ToList();
                dl_location_type.DataBind();

                dl_sports.DataSource = lstListing.Where(x => x.subType == "SPORTS").ToList();
                dl_sports.DataBind();
            }

        }

        protected void dl_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                CheckBox chk = e.Item.FindControl("chk") as CheckBox;
                DropDownList drp_count = e.Item.FindControl("drp_count") as DropDownList;
                var currRl = dcChnl.dbRntChnlHomeAwayEstateFeaturesRLs.SingleOrDefault(x=>x.pidEstate==IdEstate && x.type==featureType && x.code==lbl_id.Text);
                if (currRl != null)
                {
                    chk.Checked = true;
                    drp_count.setSelectedValue(currRl.count);
                }
            }
        }
        protected void saveData(DataListItem item, DCchnlHomeAway dcChnl)
        {
            Label lbl_id = item.FindControl("lbl_id") as Label;
            CheckBox chk = item.FindControl("chk") as CheckBox;
            DropDownList drp_count = item.FindControl("drp_count") as DropDownList;
            if (chk.Checked)
            {
                var currRl = dcChnl.dbRntChnlHomeAwayEstateFeaturesRLs.SingleOrDefault(x => x.pidEstate == IdEstate && x.type == featureType && x.code == lbl_id.Text);
                if (currRl == null)
                {
                    currRl = new dbRntChnlHomeAwayEstateFeaturesRL();
                    currRl.pidEstate = IdEstate;
                    currRl.type = featureType; 
                    currRl.code = lbl_id.Text;
                    dcChnl.Add(currRl);
                }
                currRl.count = drp_count.SelectedValue.objToInt32();
                dcChnl.SaveChanges();
            }
        }
        protected void saveData()
        {
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            {
                dcChnl.Delete(dcChnl.dbRntChnlHomeAwayEstateFeaturesRLs.Where(x => x.pidEstate == IdEstate && x.type == featureType));
                dcChnl.SaveChanges();
                foreach (DataListItem item in dl_attractios.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_car.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_leisure.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_local_features.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_location_type.Items)
                    saveData(item, dcChnl);
                foreach (DataListItem item in dl_sports.Items)
                    saveData(item, dcChnl);
            }


            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);


        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            fillData();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            saveData();
        }
    }
}