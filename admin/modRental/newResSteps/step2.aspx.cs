using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental.newResSteps
{
    public partial class step2 : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        private dbRntReservationTMP TMPcurrTBL;
        protected dbRntReservationTMP currTBL
        {
            get
            {
                if (TMPcurrTBL == null)
                    using (DCmodRental dc = new DCmodRental())
                    {
                        TMPcurrTBL = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == HF_id.Value.ToInt64());
                        if (TMPcurrTBL == null)
                        {
                            CloseRadWindow("");
                            currTBL = new dbRntReservationTMP();
                            UpdatePanel1.Visible = false;
                        }
                    }
                return TMPcurrTBL;
            }
            set
            {
                TMPcurrTBL = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                HF_id.Value = Request.QueryString["id"].ToInt64() + "";
                var tmpTBL = new dbRntReservationTMP();
                using (DCmodRental dc = new DCmodRental())
                {
                    tmpTBL = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == HF_id.Value.ToInt64());
                    if (tmpTBL == null)
                    {
                        CloseRadWindow("");
                        currTBL = new dbRntReservationTMP();
                        UpdatePanel1.Visible = false;
                        return;
                    }
                    var currEstate = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == tmpTBL.pidEstate);
                    if (currEstate == null)
                    {
                        CloseRadWindow("");
                        currTBL = new dbRntReservationTMP();
                        UpdatePanel1.Visible = false;
                        return;
                    }
                    ucPrice.fillData(tmpTBL, currEstate.pr_percentage.objToInt32(), true);
                }
            }
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                var tmpTBL = dc.dbRntReservationTMPs.SingleOrDefault(x => x.id == HF_id.Value.ToInt64());
                if (tmpTBL == null)
                {
                    CloseRadWindow("");
                    currTBL = new dbRntReservationTMP();
                    UpdatePanel1.Visible = false;
                    return;
                }
                ucPrice.saveData(ref tmpTBL);
                tmpTBL.pr_isManual = 1;
                dc.SaveChanges();
                Response.Redirect("step3.aspx?id=" + tmpTBL.id);
            }
        }

    }
}