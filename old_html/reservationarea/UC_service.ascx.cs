using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ModRental;

namespace RentalInRome.reservationarea
{
    public partial class UC_service : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                filldata();
            }
        }
        protected void filldata()
        {
            if (Session["services"] != null)
            {
                List<util_serviceAdd.Services> lstServices = (List<util_serviceAdd.Services>)Session["Services"];
                if (lstServices != null && lstServices.Count > 0)
                {
                    LV.DataSource = Session["services"];
                    LV.DataBind();
                    
                }
                else
                {
                    div_service.Visible = false;
                }
            }
            else
            {
                div_service.Visible = false;
            }
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                Label lbl_date = e.Item.FindControl("lbl_date") as Label;
                if (lbl_id == null) return;
                if (Session["services"] != null)
                {
                    List<util_serviceAdd.Services> lstServices = (List<util_serviceAdd.Services>)Session["Services"];
                    util_serviceAdd.Services objService = lstServices.SingleOrDefault(x => x.serviceId == lbl_id.Text.objToInt32() && x.date == lbl_date.Text);
                    lstServices.Remove(objService);
                    Session["services"] = lstServices;
                    filldata();

                }

            }
        }
        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                Label lbl_price = e.Item.FindControl("lbl_price") as Label;
                Label lbl_commission = e.Item.FindControl("lbl_commission") as Label;
                HtmlGenericControl div_payNow = e.Item.FindControl("div_payNow") as HtmlGenericControl;
                util_serviceAdd.Services currService = (util_serviceAdd.Services)e.Item.DataItem;
                if (currService != null)
                {
                    using (DCmodRental dc = new DCmodRental())
                    {
                        //for anticipo message
                        dbRntEstateExtrasTB currExtra = dc.dbRntEstateExtrasTBs.SingleOrDefault(x => x.id == lbl_id.Text.objToInt32());
                        if (currExtra != null)
                        {
                            if (currExtra.isInstantPayment == 0)
                            {
                                div_payNow.Visible = false;
                                lbl_price.Text = currService.price;
                            }
                            else
                            {
                                lbl_price.Text = currService.price;
                                lbl_commission.Text = currService.commission;
                            }
                        }
                    }
                }
            }
        }
    }
}