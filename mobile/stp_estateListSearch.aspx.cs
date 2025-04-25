using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.mobile
{
    public partial class stp_estateListSearch : mainBasePage
    {
        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage mb = (mainBasePage)this.Page;
                return mb.CURRENT_SESSION_ID.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clConfig _config = clUtils.getConfig(this.CURRENT_SESSION_ID);
                clSearch _ls = _config.lastSearch;

                // imposta le date
                HF_dtStart.Value = _ls.dtStart.JSCal_dateToString();
                HF_dtEnd.Value = _ls.dtEnd.JSCal_dateToString();

                // imposta le persone
                int min = AppSettings.RNT_TB_ESTATE.Select(x => x.num_persons_min.objToInt32()).Min();
                if (min == 0) min = 1;
                int max = AppSettings.RNT_TB_ESTATE.Select(x => x.num_persons_max.objToInt32()).Max();
                int maxChildMin = AppSettings.RNT_TB_ESTATE.Select(x => x.num_persons_child.objToInt32()).Max();
                HF_numPers_adult.Value = "" + _ls.numPers_adult;
                HF_numPers_childOver.Value = "" + _ls.numPers_childOver;
                HF_numPers_childMin.Value = "" + _ls.numPers_childMin;

                HF_searchTitle.Value = _ls.searchTitle;

                HF_currCity.Value = AppSettings.RNT_currCity + "";
                HF_currZoneList.Value = _ls.currZoneList.listToString("|");

                HF_currConfigList.Value = _ls.currConfigList.listToString("|");

                // slider
                HF_prRange.Value = _ls.prMin + "|" + _ls.prMax;
                HF_voteRange.Value = _ls.voteMin + "|" + _ls.voteMax;

                // OrderByHow
                HF_orderBy.Value = _ls.orderBy;
                HF_orderHow.Value = _ls.orderHow;
            }
        }
    }
}