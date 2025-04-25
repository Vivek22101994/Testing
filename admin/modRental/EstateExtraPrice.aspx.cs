using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraPrice : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        private dbRntExtrasPriceTBL currTBL;
        private List<dbRntExtrasPriceTBL> currTBLList;
        private List<dbRntExtraPriceTypesLN> currTBLListLN;
        private List<dbRntExtraPriceTypesLN> currTBLListLNFinal = new List<dbRntExtraPriceTypesLN>();
        public enum CommisssionType
        {
            Commission = 0,
            Fixed = 1
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                HfId.Value = Request.QueryString["id"].ToInt64().ToString();
                fillPrice();
                //fillGroup();

            }
        }
        private void filldrpCommission()
        {
            drp_commissionType.Items.Clear();
            drp_commissionType.Items.Add(new ListItem(CommisssionType.Commission.ToString(), (Convert.ToInt32(CommisssionType.Commission)).ToString()));
            drp_commissionType.Items.Add(new ListItem(CommisssionType.Fixed.ToString(), (Convert.ToInt32(CommisssionType.Fixed)).ToString()));
        }

        private void fillPrice()
        {
            drp_max_pax.SelectedValue = "0";
            drp_min_pax.SelectedValue = "0";
           // drp_priceType.SelectedValue = "Gratis";
            txt_price.Text = "";
            drp_hours.SelectedValue = "0";
            drp_days.SelectedValue = "0";
            hfd_price.Value = "0";
            txt_price.Text = "0,00";
            txt_commisssion.Text = "0,00";
            txt_costPrice.Text = "0,00";
            txt_costPrice_child.Text = "0,00";
            txt_price_child.Text = "0,00";
            //txt_price.Enabled = true;
            //txt_commisssion.Enabled = false;
            //drp_commissionType.Enabled = false;
            filldrpCommission();

            using (DCmodRental dc = new DCmodRental())
            {

                dbRntEstateExtrasLN currExtra = dc.dbRntEstateExtrasLNs.SingleOrDefault(x => x.pidEstateExtras == HfId.Value.ToInt32() && x.pidLang == 1);
                List<dbRntExtraPriceTypesTB> lstRntExtraPriceTB = dc.dbRntExtraPriceTypesTBs.Where(x => x.pidExtra == HfId.Value.ToInt32()).ToList();

                foreach (dbRntExtraPriceTypesTB objExtraPriceType in lstRntExtraPriceTB)
                {
                    currTBLListLN = dc.dbRntExtraPriceTypesLNs.Where(x => x.pidPriceType == objExtraPriceType.id && x.pidLang == 1).ToList();
                    currTBLListLNFinal.AddRange(currTBLListLN);
                }
                drp_priceType.Items.Clear();
                drp_priceType.Items.Add(new ListItem("selezionare", "selezionare"));
                if (currTBLListLNFinal != null && currTBLListLNFinal.Count > 0)
                {
                    
                    foreach(dbRntExtraPriceTypesLN objExtraPriceLn in  currTBLListLNFinal)
                    {
                        drp_priceType.Items.Add(new ListItem(objExtraPriceLn.title, Convert.ToString(objExtraPriceLn.pidPriceType)));
                    }
                }
                ltrTitle.Text = "Aggiunta di prezzo #" + currExtra.title;
                fillLV();
            }

            //drp_price_group.SelectedValue = "0";


        }

        private void fillLV()
        {

            using (DCmodRental dc = new DCmodRental())
            {

                currTBLList = dc.dbRntExtrasPriceTBLs.Where(x => x.pidExtras == HfId.Value.ToInt32()).ToList();
                if (currTBLList != null && currTBLList.Count > 0)
                {
                    LVPrice.DataSource = currTBLList;
                    LVPrice.DataBind();
                }
                else
                {
                    LVPrice.DataSource = null;
                    LVPrice.DataBind();
                }
            }

            //drp_price_group.SelectedValue = "0";


        }

        private void SavePrice()
        {
            if (drp_min_pax.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Persona minimo è richiesto.\", 340, 110);", true);
            }
            else if (drp_max_pax.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"massimo Persona è richiesto.\", 340, 110);", true);
            }
            else if (Convert.ToInt32(drp_min_pax.SelectedValue) > Convert.ToInt32(drp_max_pax.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"valore massimo persona dovrebbe essere greatre di valore minimo persona.\", 340, 110);", true);
            }
            else if (txt_price.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Prezzo è tenuta.\", 340, 110);", true);
            }
            else if (txt_commisssion.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"è richiesto commissione.\", 340, 110);", true);
            }

            else
            {

                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntExtrasPriceTBLs.SingleOrDefault(x => x.id == hfd_price.Value.ToInt32());
                    if (currTBL == null)
                    {
                        currTBL = new dbRntExtrasPriceTBL();
                        dc.Add(currTBL);
                    }

                    //currPriceTBL.pidCarRent=HfId.Value.ToInt32();
                    //currPriceTBL.GroupId = Convert.ToInt32(drp_price_group.SelectedValue);
                    currTBL.minPax = Convert.ToInt32(drp_min_pax.SelectedValue);
                    currTBL.maxPax = Convert.ToInt32(drp_max_pax.SelectedValue);
                    if (drp_days.SelectedValue != "0")
                        currTBL.Days = Convert.ToInt32(drp_days.SelectedValue);
                    else
                        currTBL.Days = null;
                    if (drp_hours.SelectedValue != "0")
                        currTBL.Hours = Convert.ToInt32(drp_hours.SelectedValue);
                    else
                        currTBL.Hours = null;
                    currTBL.actualPrice = txt_price.Text.objToDecimal();
                    currTBL.actualPriceChild = txt_price_child.Text.objToDecimal();
                    currTBL.pidExtras = Convert.ToInt32(HfId.Value);
                    currTBL.priceType = Convert.ToString(drp_priceType.SelectedValue);
                    currTBL.paymentType = Convert.ToString(drp_PaymentType.SelectedValue);
                    currTBL.CommissionType = Convert.ToInt32(drp_commissionType.SelectedValue);
                    currTBL.Commission = txt_commisssion.Value.objToDecimal();
                    currTBL.costPrice = txt_costPrice.Value.objToDecimal();
                    currTBL.costPriceChild = txt_costPrice_child.Value.objToDecimal();
                    dc.SaveChanges();
                    fillPrice();

                }
            }


        }


        protected void LvPrice_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                dbRntExtrasPriceTBL currTBLPrice = (dbRntExtrasPriceTBL)e.Item.DataItem;

                Label lbl_commissionType = (Label)e.Item.FindControl("lbl_commissionType");
                Label lbl_costPrice = (Label)e.Item.FindControl("lbl_costPrice");
                Label lbl_child_costPrice = (Label)e.Item.FindControl("lbl_child_costPrice");
                
                foreach (CommisssionType objCommission in Enum.GetValues(typeof(CommisssionType)))
                {
                    if (Convert.ToInt32(objCommission) == currTBLPrice.CommissionType)
                    {

                        lbl_commissionType.Text = objCommission.ToString();
                    }

                }

                

            }


        }

        protected void LvPrice_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

            Label lbl_id = (Label)e.Item.FindControl("lbl_id");
            int id = Convert.ToInt32(lbl_id.Text);


            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntExtrasPriceTBLs.SingleOrDefault(x => x.id == id);

                if (e.CommandName == "DeletePrice")
                {
                    LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                    //if (lnkDelete.Text == "attivare")
                    //{
                    //    currTBLDelete.isActive = 0;
                    //}
                    //else if (lnkDelete.Text == "disattivare")
                    //{
                    //    currTBLDelete.isActive = 1;
                    //}
                    dc.Delete(currTBL);
                    dc.SaveChanges();
                    fillPrice();
                }
                if (e.CommandName == "EditPrice")
                {
                    hfd_price.Value = Convert.ToString(id);
                    txt_costPrice.Value = Convert.ToDouble(currTBL.costPrice);
                    txt_costPrice_child.Value = Convert.ToDouble(currTBL.costPriceChild);
                    drp_min_pax.setSelectedValue(currTBL.minPax);
                    drp_max_pax.setSelectedValue(currTBL.maxPax);
                    drp_priceType.setSelectedValue(currTBL.priceType);
                    drp_days.setSelectedValue(currTBL.Days);
                    drp_PaymentType.setSelectedValue(currTBL.paymentType);
                    drp_hours.setSelectedValue(currTBL.Hours);
                    drp_commissionType.setSelectedValue(currTBL.CommissionType);                            
                    txt_commisssion.Value = Convert.ToDouble(currTBL.Commission);
                    CalucateCostPrice();

                }
            }



        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            SavePrice();

            //CloseRadWindow("reload");
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillPrice();
        }
        //protected void drp_PaymentType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (drp_priceType.SelectedValue == "Gratis")
        //    {
        //        txt_price.Text = "0.00";
        //        txt_price.Enabled = false;
        //        txt_commisssion.Text = "0.00";
        //        txt_commisssion.Enabled = false;
        //        txt_costPrice.Text = "0.00";
        //        drp_commissionType.Enabled = false;


        //    }
        //    else
        //    {
        //        txt_price.Enabled = true;
        //        txt_commisssion.Enabled = true;
        //        drp_commissionType.Enabled = true;
        //    }
        //    fillLV();
        //}

        private void CalucateCostPrice()
        {
            string type = drp_commissionType.SelectedValue;
            decimal price = txt_costPrice.Value.objToDecimal();
            decimal childPrice = txt_costPrice_child.Value.objToDecimal();
            decimal commissionper = txt_commisssion.Value.objToDecimal();
            decimal commission = 0;
            decimal costPrice = 0;
            decimal childcostPrice = 0;
            if (txt_costPrice.Text != "" && txt_commisssion.Text != "")
            {
                if (type == "0")
                {
                    commission = (price * commissionper) / 100;
                    costPrice = price + commission;
                }
                else
                {
                    costPrice = price + commissionper;
                }

                txt_price.Text = costPrice.ToString("N2");

            }

            if (txt_costPrice_child.Text != "" && txt_commisssion.Text != "")
            {
                if (type == "0")
                {
                    commission = (childPrice * commissionper) / 100;
                    childcostPrice = childPrice + commission;
                }
                else
                {
                    childcostPrice = childPrice + commissionper;
                }

                txt_price_child.Text = childcostPrice.ToString("N2");

            }
        }


    }

}