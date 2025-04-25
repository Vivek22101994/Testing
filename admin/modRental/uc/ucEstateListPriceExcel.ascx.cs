using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace ModRental.admin.modRental.uc
{
    public partial class ucEstateListPriceExcel : System.Web.UI.UserControl
    {
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE _currTBL;
        public int discount
        {
            get
            {
                return HF_discount.Value.ToInt32();
            }
            set
            {
                HF_discount.Value = value+"";
            }
        }
        public string FILTER
        {
            get
            {
                return HF_lds_filter.Value;
            }
            set
            {
                DC_RENTAL = maga_DataContext.DC_RENTAL;
                HF_lds_filter.Value = value;
                FillList();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
        }
        class TmpPriceList
        {
            public int pax { get; set; }
            public decimal pr1d { get; set; }
            public decimal pr1w { get; set; }
            public decimal pr2d { get; set; }
            public decimal pr2w { get; set; }
            public decimal pr3d { get; set; }
            public decimal pr3w { get; set; }
            public TmpPriceList(int Pax)
            {
                pax = Pax;
            }
        }
        private void FillList()
        {
            LDS.Where = HF_lds_filter.Value;
            LDS.DataBind();
            LV.DataBind();
            foreach (ListViewDataItem item in LV.Items)
            {
                var lbl_id = item.FindControl("lbl_id") as Label;
                var LvInner = item.FindControl("LvInner") as ListView;
                var IdEstate = lbl_id.Text.ToInt32();
                var currEstateTB = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstateTB == null)
                {
                    item.Visible = false;
                    continue;
                }
                List<TmpPriceList> prList = new List<TmpPriceList>();
                decimal _pr_discount7daysApply = 1 - (currEstateTB.pr_discount7days.objToDecimal() / 100);
                decimal _pr_discount30daysApply = 1 - (currEstateTB.pr_discount30days.objToDecimal() / 100);
                int pr_basePersons = currEstateTB.pr_basePersons.objToInt32();
                for (int i = pr_basePersons; i <= currEstateTB.num_persons_max.objToInt32(); i++)
                {
                    int extraPersons = i - pr_basePersons;

                    var pr = new TmpPriceList(i);

                    pr.pr1d = currEstateTB.pr_1_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_1_opt.objToDecimal());
                    pr.pr1w = (pr.pr1d * 7 *_pr_discount7daysApply).customRound(true);

                    pr.pr2d = currEstateTB.pr_2_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_2_opt.objToDecimal());
                    pr.pr2w = (pr.pr2d * 7 *_pr_discount7daysApply).customRound(true);

                    pr.pr3d = currEstateTB.pr_3_2pax.objToDecimal() + (extraPersons * currEstateTB.pr_3_opt.objToDecimal());
                    pr.pr3w = (pr.pr3d * 7 *_pr_discount7daysApply).customRound(true);

                    if (discount > 0)
                    {
                        pr.pr1d = (pr.pr1d - pr.pr1d * discount / 100).customRound(true);
                        pr.pr1w = (pr.pr1w - pr.pr1w * discount / 100).customRound(true);

                        pr.pr2d = (pr.pr2d - pr.pr2d * discount / 100).customRound(true);
                        pr.pr2w = (pr.pr2w - pr.pr2w * discount / 100).customRound(true);

                        pr.pr3d = (pr.pr3d - pr.pr3d * discount / 100).customRound(true);
                        pr.pr3w = (pr.pr3w - pr.pr3w * discount / 100).customRound(true);
                    }

                    prList.Add(pr);
                }
                LvInner.DataSource = prList;
                LvInner.DataBind();

            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ListinoPrezzi.xls");
            Response.Charset = "charset=utf-8";
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            LV.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();

        }
    }
}
