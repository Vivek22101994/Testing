using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_rnt_residence_navlinks : System.Web.UI.UserControl
    {
        public int IdResidence
        {
            get
            {
                return HF_id.Value.ToInt32();
            }
            set
            {
                HF_id.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected string getDetailClass()
        {
            //List<RNT_LN_ESTATE> _rList = AppSettings.RNT_LN_ESTATE.Where(x => x.pid_estate == IdResidence).ToList();
            //RNT_LN_ESTATE _lang = _rList.FirstOrDefault(x => x.title == null || x.title.Trim() == "" || x.meta_title == null || x.meta_title.Trim() == "" || x.meta_description == null || x.meta_description.Trim() == "" || x.description == null || x.description.Trim() == "");
            //if (_rList.Count == 0 || _rList.Count < AppSettings.CONT_LANGs.Count)
            //    return "alert1";
            //if (_lang != null)
            //    return "alert2";
            return "";
        }
    }
}