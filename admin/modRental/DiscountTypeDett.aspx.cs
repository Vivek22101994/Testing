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
    public partial class DiscountTypeDett : adminBasePage
    {
        protected dbRntDiscountTypeTBL currTBL;
        protected int CurrID
        {
            get { return HfId.Value.ToInt32(); }
            set { HfId.Value = value.ToString(); }
        }
        private dbRntDiscountTypeTBL TMPcurrFases;
        public dbRntDiscountTypeTBL currFases
        {
            get
            {
                //if (TMPcurrFases == null)
                //    TMPcurrFases = (dbRntDiscountTypeTBL)ViewState["currFases"];
                return TMPcurrFases ?? new dbRntDiscountTypeTBL();
            }
            set { TMPcurrFases = value;// ViewState["currFases"] = TMPcurrFases; 
            }
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
                currTBL = dc.dbRntDiscountTypeTBLs.SingleOrDefault(x => x.id == CurrID);
                if (currTBL == null)
                {
                    CurrID = 0;
                    currTBL = new dbRntDiscountTypeTBL();
                    //currTBL.fase1_discount = 0;
                    //currTBL.fase1_end = 0;
                    //currTBL.fase1_start = 0;
                    //currTBL.fase2_discount = 0;
                    //currTBL.fase2_end = 0;
                    //currTBL.fase2_start = 0;
                    //currTBL.fase3_discount = 0;
                    //currTBL.fase3_end = 0;
                    //currTBL.fase3_start = 0;
                    //currTBL.fase4_discount = 0;
                    //currTBL.fase4_end = 0;
                    //currTBL.fase4_start = 0;
                    //currTBL.fase5_discount = 0;
                    //currTBL.fase5_end = 0;
                    //currTBL.fase5_start = 0;
                    //currTBL.fase6_discount = 0;
                    //currTBL.fase6_end = 0;
                    //currTBL.fase6_start = 0;
                    //currTBL.fase7_discount = 0;
                    //currTBL.fase7_end = 0;
                    //currTBL.fase7_start = 0;
                    //currTBL.fase8_discount = 0;
                    //currTBL.fase8_end = 0;
                    //currTBL.fase8_start = 0;
                    //currTBL.fase9_discount = 0;
                    //currTBL.fase9_end = 0;
                    //currTBL.fase9_start = 0;
                    //currTBL.fase10_discount = 0;
                    //currTBL.fase10_end = 0;
                    //currTBL.fase10_start = 0;
                    ltrTitle.Text = "Nuova Tipologia di Sconti";
                }
                else
                    ltrTitle.Text = "Scheda Tipologia di Sconti #:" + currTBL.code;

                txt_code.Text = currTBL.code;
                //txt_cashDiscount.Value = Convert.ToDouble(currTBL.cashDiscount);
                re_description.Content = currTBL.description;

                ntxt_fase1_discount.Value = currTBL.fase1_discount.objToDouble();

                ntxt_fase2_start.Value = currTBL.fase2_start.objToDouble();
                ntxt_fase2_discount.Value = currTBL.fase2_discount.objToDouble();

                ntxt_fase3_start.Value = currTBL.fase3_start.objToDouble();
                ntxt_fase3_discount.Value = currTBL.fase3_discount.objToDouble();

                ntxt_fase4_start.Value = currTBL.fase4_start.objToDouble();
                ntxt_fase4_discount.Value = currTBL.fase4_discount.objToDouble();

                ntxt_fase5_start.Value = currTBL.fase5_start.objToDouble();
                ntxt_fase5_discount.Value = currTBL.fase5_discount.objToDouble();

                ntxt_fase6_start.Value = currTBL.fase6_start.objToDouble();
                ntxt_fase6_discount.Value = currTBL.fase6_discount.objToDouble();

                ntxt_fase7_start.Value = currTBL.fase7_start.objToDouble();
                ntxt_fase7_discount.Value = currTBL.fase7_discount.objToDouble();

                ntxt_fase8_start.Value = currTBL.fase8_start.objToDouble();
                ntxt_fase8_discount.Value = currTBL.fase8_discount.objToDouble();

                ntxt_fase9_start.Value = currTBL.fase9_start.objToDouble();
                ntxt_fase9_discount.Value = currTBL.fase9_discount.objToDouble();

                ntxt_fase10_start.Value = currTBL.fase10_start.objToDouble();
                ntxt_fase10_discount.Value = currTBL.fase10_discount.objToDouble();

                currFases = currTBL;
            }
        }
        private void saveData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTBL = dc.dbRntDiscountTypeTBLs.SingleOrDefault(x => x.id == CurrID);
                if (currTBL == null)
                {
                    currTBL = new dbRntDiscountTypeTBL();
                    dc.Add(currTBL);
                    dc.SaveChanges();
                }
                currTBL.code = txt_code.Text;
                currTBL.description = re_description.Content;

                currTBL.fase1_discount = ntxt_fase1_discount.Value.objToDecimal();
                currTBL.fase1_end = ntxt_fase2_start.Value.objToInt32() - 1;
                currTBL.fase1_start = 0;

                currTBL.fase2_discount = ntxt_fase2_discount.Value.objToDecimal();
                currTBL.fase2_end = ntxt_fase3_start.Value.objToInt32() - 1;
                currTBL.fase2_start = ntxt_fase2_start.Value.objToInt32();

                currTBL.fase3_discount = ntxt_fase3_discount.Value.objToDecimal();
                currTBL.fase3_end = ntxt_fase4_start.Value.objToInt32() - 1;
                currTBL.fase3_start = ntxt_fase3_start.Value.objToInt32();

                currTBL.fase4_discount = ntxt_fase4_discount.Value.objToDecimal();
                currTBL.fase4_end = ntxt_fase5_start.Value.objToInt32() - 1;
                currTBL.fase4_start = ntxt_fase4_start.Value.objToInt32();

                currTBL.fase5_discount = ntxt_fase5_discount.Value.objToDecimal();
                currTBL.fase5_end = ntxt_fase6_start.Value.objToInt32() - 1;
                currTBL.fase5_start = ntxt_fase5_start.Value.objToInt32();

                currTBL.fase6_discount = ntxt_fase6_discount.Value.objToDecimal();
                currTBL.fase6_end = ntxt_fase7_start.Value.objToInt32() - 1;
                currTBL.fase6_start = ntxt_fase6_start.Value.objToInt32();

                currTBL.fase7_discount = ntxt_fase7_discount.Value.objToDecimal();
                currTBL.fase7_end = ntxt_fase8_start.Value.objToInt32() - 1;
                currTBL.fase7_start = ntxt_fase7_start.Value.objToInt32();

                currTBL.fase8_discount = ntxt_fase8_discount.Value.objToDecimal();
                currTBL.fase8_end = ntxt_fase9_start.Value.objToInt32() - 1;
                currTBL.fase8_start = ntxt_fase8_start.Value.objToInt32();

                currTBL.fase9_discount = ntxt_fase9_discount.Value.objToDecimal();
                currTBL.fase9_end = ntxt_fase10_start.Value.objToInt32() - 1;
                currTBL.fase9_start = ntxt_fase9_start.Value.objToInt32();

                currTBL.fase10_discount = ntxt_fase10_discount.Value.objToDecimal();
                currTBL.fase10_end = 0;
                currTBL.fase10_start = ntxt_fase10_start.Value.objToInt32();


                //currTBL.fase1_discount = currFases.fase1_discount;
                //currTBL.fase1_end = currFases.fase1_end;
                //currTBL.fase1_start = currFases.fase1_start;

                //currTBL.fase2_discount = currFases.fase2_discount;
                //currTBL.fase2_end = currFases.fase2_end;
                //currTBL.fase2_start = currFases.fase2_start;

                //currTBL.fase3_discount = currFases.fase3_discount;
                //currTBL.fase3_end = currFases.fase3_end;
                //currTBL.fase3_start = currFases.fase3_start;

                //currTBL.fase4_discount = currFases.fase4_discount;
                //currTBL.fase4_end = currFases.fase4_end;
                //currTBL.fase4_start = currFases.fase4_start;

                //currTBL.fase5_discount = currFases.fase5_discount;
                //currTBL.fase5_end = currFases.fase5_end;
                //currTBL.fase5_start = currFases.fase5_start;

                //currTBL.fase6_discount = currFases.fase6_discount;
                //currTBL.fase6_end = currFases.fase6_end;
                //currTBL.fase6_start = currFases.fase6_start;

                //currTBL.fase7_discount = currFases.fase7_discount;
                //currTBL.fase7_end = currFases.fase7_end;
                //currTBL.fase7_start = currFases.fase7_start;

                //currTBL.fase8_discount = currFases.fase8_discount;
                //currTBL.fase8_end = currFases.fase8_end;
                //currTBL.fase8_start = currFases.fase8_start;

                //currTBL.fase9_discount = currFases.fase9_discount;
                //currTBL.fase9_end = currFases.fase9_end;
                //currTBL.fase9_start = currFases.fase9_start;

                //currTBL.fase10_discount = currFases.fase10_discount;
                //currTBL.fase10_end = currFases.fase10_end;
                //currTBL.fase10_start = currFases.fase10_start;
                
                dc.SaveChanges();
                rntProps.DiscountTypeTBL = dc.dbRntDiscountTypeTBLs.ToList();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            }
        }
        private void fillFaseList()
        {
            currTBL = currFases;

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


        protected void lnk_faseNew_Click(object sender, EventArgs e)
        {
            HF_faseEdit.Value = "";
            fillFase();
            pnl_faseEdit.Visible = true;
        }
        protected void lnk_faseSave_Click(object sender, EventArgs e)
        {
            saveFase();
            HF_faseEdit.Value = "";
            pnl_faseEdit.Visible = false;
        }
        protected void lnk_faseCancel_Click(object sender, EventArgs e)
        {
            HF_faseEdit.Value = "";
            pnl_faseEdit.Visible = false;
        }
        private void fillFase()
        {
            currTBL = currFases;

        }

        protected void saveFase()
        {
            currTBL = currFases;

            currFases = currTBL;
        }

    }
}

