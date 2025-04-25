using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.WLRental.uc
{
    public partial class UC_static_collection : System.Web.UI.UserControl
    {
        public string CollectionID
        {
            get
            {
                return HF_id.Value;
            }
            set
            {
                HF_id.Value = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                HF_pid_lang.Value = CurrentLang.ID.ToString();
        }
    }
}