using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.mobile
{
    public partial class rnt_reservationNotes : System.Web.UI.Page
    {
        public long IdReservation
        {
            get { return HF_id.Value.ToInt32(); }
            set { HF_id.Value = value.ToString(); }
        }
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                if (Request.QueryString["uid"] != null)
                {
                    Uri referrer = HttpContext.Current.Request.UrlReferrer;
                    if (referrer != null)
                    {
                        HL_back.NavigateUrl = referrer.OriginalString.ToLower();
                        HL_back.Visible = true;
                    }
                    else
                        HL_back.Visible = false;
                    //Guid _unique = new Guid(Request.QueryString["uid"]);
                    //_currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.uid_2 == _unique);
                    _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == Request.QueryString["id"].objToInt64());
                    if (_currTBL != null && Request.QueryString["uid"] == "cfab20dd-3b2c-4e57-ba76-597cfa8f450e")
                    {
                        IdReservation = _currTBL.id;
                        fillData();
                        return;
                    }
                }
                Response.Clear();
                Response.End();
                return;
            }
        }
        public void fillData()
        {
            HF_code.Value = _currTBL.code;

            txt_body.Text = _currTBL.notesInner;
        }
    }
}