using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlHA_price : adminBasePage
    {
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

        protected string featureType = "Price";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";            
        }

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
                FillControls();
            }
        }

        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
      
        protected void FillControls()
        {
            using (DCchnlHomeAway dc = new DCchnlHomeAway())
            {
                var currTbl = dc.dbRntChnlHomeAwayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                
                ntxt_changeAmount.Value = currTbl.changeAmount.objToInt32();                
                drp_changeIsDiscount.setSelectedValue(currTbl.changeIsDiscount.objToInt32());                
                drp_changeIsPercentage.setSelectedValue(currTbl.changeIsPercentage.objToInt32());
            } 
        }

        protected void FillDataFromControls()
        {
            using (DCchnlHomeAway dc = new DCchnlHomeAway())
            {
                var currTbl = dc.dbRntChnlHomeAwayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                currTbl.changeAmount = ntxt_changeAmount.Value.objToInt32();
                currTbl.changeIsDiscount = drp_changeIsDiscount.getSelectedValueInt();                
                currTbl.changeIsPercentage = drp_changeIsPercentage.getSelectedValueInt();                
                dc.SaveChanges();                 
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
            }
            FillControls();
        }       
        
    }
}