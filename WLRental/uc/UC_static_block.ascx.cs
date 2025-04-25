using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.WLRental.uc
{
    public partial class UC_static_block : System.Web.UI.UserControl
    {
        public string BlockID
        {
            get
            {
                return HF_id.Value;
            }
            set
            {
                HF_id.Value = value.ToString();
                Fill_controls();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Fill_controls()
        {
            int id;
            if (Int32.TryParse(HF_id.Value, out id))
            {
                CONT_VIEW_TBL_BLOCK _block = maga_DataContext.DC_CONTENT.CONT_VIEW_TBL_BLOCKs.SingleOrDefault(x => x.id == id && x.pid_lang == CurrentLang.ID);
                if (_block != null && _block.description != null && _block.description.Trim() != "")
                    ltr_description.Text = _block.description;
                else
                    Visible = false;
            }
        }
    }
}