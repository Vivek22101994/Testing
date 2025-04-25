using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class DiscountPromoCodeDett : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "RntDiscountPromoCode";
        }
        protected dbRntDiscountPromoCodeTBL currTBL;
        protected int CurrID
        {
            get { return HfId.Value.ToInt32(); }
            set { HfId.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrID = Request.QueryString["id"].ToInt32();
                fillData();
            }
        }
        private void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntDiscountPromoCodeTBLs.SingleOrDefault(x => x.id == CurrID);
                if (currTBL == null)
                {
                    CurrID = 0;
                    currTBL = new dbRntDiscountPromoCodeTBL();
                    ltrTitle.Text = "Nuovo Sconto PromoCode";
                }
                else
                    ltrTitle.Text = "Scheda Sconto PromoCode #:" + currTBL.code;

                txt_code.Text = currTBL.code;
                ntxt_discountAmount.Value = currTBL.discountAmount.objToDouble();
                drp_isActive.setSelectedValue(currTBL.isActive);
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntDiscountPromoCodeTBLs.SingleOrDefault(x => x.id == CurrID);
                if (currTBL == null)
                {
                    currTBL = new dbRntDiscountPromoCodeTBL();
                    dc.Add(currTBL);
                    dc.SaveChanges();
                }
                currTBL.code = txt_code.Text;
                currTBL.discountAmount = ntxt_discountAmount.Value.objToDecimal();
                currTBL.isActive = drp_isActive.getSelectedValueInt();

                
                dc.SaveChanges();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }


    }
}

