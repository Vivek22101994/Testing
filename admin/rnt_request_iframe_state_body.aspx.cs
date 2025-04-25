using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_request_iframe_state_body : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_request";
        }
        private RNT_RL_REQUEST_STATE _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    _currTBL = maga_DataContext.DC_RENTAL.RNT_RL_REQUEST_STATEs.SingleOrDefault(x => x.id == _id);
                    if (_currTBL == null)
                        return;
                    ltr_body.Text = _currTBL.body;
                }
            }
        }
    }
}
