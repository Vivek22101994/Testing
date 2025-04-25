using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using System.Web.UI.HtmlControls;

namespace RentalInRome.reservationarea
{
    public partial class UC_reservedService : System.Web.UI.UserControl
    {
        public long CurrentIdReservation
        {
            get
            {
                basePage m = (basePage)this.Page;
                return m.CurrentIdReservation;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                filldata();
            }
        }
        protected void filldata()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                List<dbRntReservationExtrasTMP> currRes = dc.dbRntReservationExtrasTMPs.Where(x => x.pidReservation == CurrentIdReservation).ToList();
                if (currRes != null && currRes.Count > 0)
                {
                    LV.DataSource = currRes;
                    LV.DataBind();
                }


                else
                {
                    div_service.Visible = false;
                }
            }
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_name = (Label)e.Item.FindControl("lbl_name");
            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            Label lbl_date = (Label)e.Item.FindControl("lbl_date");
            Label lbl_price = e.Item.FindControl("lbl_price") as Label;
            Label lbl_commission = e.Item.FindControl("lbl_commission") as Label;
            HtmlGenericControl div_payNow = e.Item.FindControl("div_payNow") as HtmlGenericControl;

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    dbRntReservationExtrasTMP currRes = (dbRntReservationExtrasTMP)e.Item.DataItem;
                    if (currRes != null)
                    {
                        DateTime dt = currRes.inDate.Value;

                        lbl_date.Text = dt.formatCustom("#dd# #MM# #yy#", CurrentLang.ID, "");
                        dbRntEstateExtrasLN objEstateExtrasLN = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == currRes.pidExtras && x.pidLang == App.LangID);
                        if (objEstateExtrasLN == null || string.IsNullOrEmpty(objEstateExtrasLN.title))
                            objEstateExtrasLN = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == currRes.pidExtras && x.pidLang == 2);
                        if (objEstateExtrasLN != null)
                        {
                            lbl_name.Text = objEstateExtrasLN.title;

                        }
                       
                            //for anticipo message
                            dbRntEstateExtrasTB currExtra = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == currRes.pidExtras);
                            if (currExtra != null)
                            {
                                if (currExtra.isInstantPayment == 0)
                                {
                                    div_payNow.Visible = false;
                                    lbl_price.Text = Convert.ToString(currRes.Price);
                                }
                                else
                                {
                                    lbl_price.Text = Convert.ToString(currRes.Price);
                                    lbl_commission.Text = Convert.ToString(currRes.Commission);
                                }
                            }
                    
                        

                    }
                }
            }
        }
    }
}