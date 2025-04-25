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
    public partial class EstateChlnHA_cancallationPolicy : adminBasePage
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
                FillControls();
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
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            {
                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                var currTbl = dcChnl.dbRntChnlHomeAwayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                txt_description.Text = currTbl.CancellationPolicyDescription;
                ntxt_days.Value = currTbl.CancellationPolicyDeadlineDaysBeforeCheckIn;
                //drp_isDiscount.setSelectedValue(currTbl.CancellationPolicyChargeType);
                drp_penalty_type.setSelectedValue(currTbl.CancellationPolicyType);
                ntxt_amount.Value = currTbl.CancellationPolicyCharge.objToDouble();
            }
        }


        protected void FillDataFromControls()
        {
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            {
                var currTbl = dcChnl.dbRntChnlHomeAwayEstateTBs.SingleOrDefault(x => x.id == IdEstate);
                if (currTbl == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }

                currTbl.CancellationPolicyDescription = txt_description.Text;
                currTbl.CancellationPolicyDeadlineDaysBeforeCheckIn = ntxt_days.Value.objToInt32();
                //currTbl.CancellationPolicyChargeType = drp_isDiscount.getSelectedValueInt();
                currTbl.CancellationPolicyType = drp_penalty_type.SelectedValue;
                currTbl.CancellationPolicyCharge = ntxt_amount.Value.objToDecimal();

                dcChnl.SaveChanges();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate correttamente.\", 340, 110);", true);
                FillControls();
            }
        }


    }
}