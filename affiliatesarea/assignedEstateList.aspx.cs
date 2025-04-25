using ModRental;
using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace RentalInRome.affiliatesarea
{
    public partial class assignedEstateList : agentBasePage
    {
        protected dbRntAgentTBL currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drp_wl_changeIsDiscount_Agent.DataBind();
                fillData();
                LV.DataSource = CURRENT_RNT_estateList;
                LV.DataBind();                
            }
        }

        private void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {                 
                currTBL = dc.dbRntAgentTBLs.FirstOrDefault(x => x.id == agentAuth.CurrentID);
            }
            if (currTBL == null)
                return;

            #region White Label Agent Details
            ntxt_wl_changeAmount_Agent.Value = currTBL.WL_changeAmount_Agent.objToInt32();
            drp_wl_changeIsDiscount_Agent.setSelectedValue(currTBL.WL_changeIsDiscount_Agent);

            //int WL_changeIsPercentage_Agent = agent.WL_changeIsPercentage_Agent == null ? 1 : agent.WL_changeIsPercentage_Agent.objToInt32(); 
            #endregion
        }

        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == agentAuth.CurrentID);
                if (currTBL == null)
                    return;

                #region White Label Agent Details
                currTBL.WL_changeIsPercentage_Agent = 1;
                currTBL.WL_changeAmount_Agent = ntxt_wl_changeAmount_Agent.Value.objToInt32();
                currTBL.WL_changeIsDiscount_Agent = drp_wl_changeIsDiscount_Agent.getSelectedValueInt();
                #endregion

                dc.SaveChanges();
                rntProps.AgentTBL = dc.dbRntAgentTBLs.ToList();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\""+ contUtils.getLabel("lblSavedData") +".\", 340, 110);", true);
                fillData();
            }
        }

        private List<AppSettings.RNT_estate> CURRENT_RNT_estateList
        {
            get
            {
                int currLangId = App.LangID;
                using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
                {   
                    List<AppSettings.RNT_estate> estateList = AppSettings.RNT_estateList.ToList();

                    using (DCmodRental dc = new DCmodRental())
                    {
                        List<int> lstAgentEstate = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgent == agentAuth.CurrentID).Select(x => x.pidEstate).Distinct().ToList();
                        estateList = estateList.Where(x => lstAgentEstate.Contains(x.id)).ToList();
                    }
                    
                    // agenzie vedono solo online_booking                   
                    estateList = estateList.Where(x => x.is_online_booking == 1).ToList();
                                      
                    List<int> removeIds = new List<int>();
                    foreach (AppSettings.RNT_estate _rntEst in estateList)
                    {
                        RNT_LN_ESTATE _lang = AppSettings.RNT_LN_ESTATE.SingleOrDefault(x => x.pid_estate == _rntEst.id && x.pid_lang == currLangId && !string.IsNullOrEmpty(x.title) && !string.IsNullOrEmpty(x.page_path));
                        if (_lang == null) { removeIds.Add(_rntEst.id); continue; }
                        _rntEst.pid_lang = currLangId;
                        _rntEst.title = _lang.title;
                        _rntEst.page_path = _lang.page_path;
                    }
                    estateList.RemoveAll(x => removeIds.Contains(x.id));
                    return estateList;
                }
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
        }
       
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }

        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }
    }
}