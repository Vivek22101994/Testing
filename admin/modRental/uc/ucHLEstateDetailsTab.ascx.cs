using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.modRental.uc
{
    public partial class ucHLEstateDetailsTab : System.Web.UI.UserControl
    {
        public int IdEstate
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    var currTbl = dc.dbRntChnlHolidayEstateTBs.SingleOrDefault(x => x.id == value);
                    if (currTbl == null)
                    {
                        this.Visible = false;
                        return;
                    }
                    else
                        this.Visible = true;
                }
                HF_id.Value = value.ToString();
                //HF_pidOwner.Value = currTbl.pid_owner.objToInt32() + "";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        protected string getDetailClass()
        {
            List<RNT_LN_ESTATE> _rList = AppSettings.RNT_LN_ESTATE.Where(x => x.pid_estate == IdEstate).ToList();
            RNT_LN_ESTATE _lang = _rList.FirstOrDefault(x => x.title == null || x.title.Trim() == "" || x.meta_title == null || x.meta_title.Trim() == "" || x.meta_description == null || x.meta_description.Trim() == "" || x.description == null || x.description.Trim() == "");
            if (_rList.Count == 0 || _rList.Count < contProps.LangTBL.Where(x => x.is_active == 1).Count())
                return "alert1";
            if (_lang != null)
                return "alert2";
            return "";
        }
    }
}