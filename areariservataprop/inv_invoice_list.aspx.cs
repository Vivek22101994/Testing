using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using EO.Pdf;
using RentalInRome.data;

namespace RentalInRome.areariservataprop
{
    public partial class inv_invoice_list : ownerBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected magaInvoice_DataContext DC_INVOICE;
        private List<INV_TBL_INVOICE> CURRENT_LIST_;
        private List<INV_TBL_INVOICE> CURRENT_LIST
        {
            get
            {
                if (CURRENT_LIST_ == null)
                    if (ViewState["CURRENT_LIST"] != null)
                    {
                        CURRENT_LIST_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_LIST"],
                                                 typeof(INV_TBL_INVOICE)).Cast<INV_TBL_INVOICE>().ToList();
                    }
                    else
                        CURRENT_LIST_ = new List<INV_TBL_INVOICE>();

                return CURRENT_LIST_;
            }
            set
            {
                ViewState["CURRENT_LIST"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_LIST_ = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            DC_INVOICE = maga_DataContext.DC_INVOICE;
            if (!IsPostBack)
            {
                if (OwnerAuthentication.CurrentID != 400)
                {
                    Response.Redirect("Default.aspx");
                    return;
                }
                HF_inv_dtInvoice_from.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).JSCal_dateToString();
                LoadContent();
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setCal", "setCal();", true);
        }
        protected void lnkFilter_Click(object sender, EventArgs e)
        {
            LoadContent();
        }
        protected void LV_PagePropertiesChanged(object sender, EventArgs e)
        {
            Fill_LV();
        }
        protected void LoadContent()
        {
            using (magaRental_DataContext dcOld = maga_DataContext.DC_RENTAL)
            {
                var estateIds = AppSettings.RNT_TB_ESTATE.Where(x => x.pid_owner == OwnerAuthentication.CurrentID).Select(x => x.id).ToList();
                var reservationIds = dcOld.RNT_TBL_RESERVATION.Where(x => x.pid_estate.HasValue && estateIds.Contains(x.pid_estate.Value)).Select(x => x.id).ToList();
                List<INV_TBL_INVOICE> _list = DC_INVOICE.INV_TBL_INVOICE.Where(x => x.rnt_pid_reservation.HasValue && reservationIds.Contains(x.rnt_pid_reservation.Value)).ToList();

                if (txt_code.Text.Trim() != "")
                {
                    _list = _list.Where(x => x.code != null && x.code.ToLower().Contains(txt_code.Text.ToLower().Trim())).ToList();
                }
                if (txt_rnt_reservation_code.Text.Trim() != "")
                {
                    _list = _list.Where(x => x.rnt_reservation_code != null && x.rnt_reservation_code.ToLower().Contains(txt_rnt_reservation_code.Text.ToLower().Trim())).ToList();
                }

                if (HF_inv_dtInvoice_from.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.inv_dtInvoice >= HF_inv_dtInvoice_from.Value.JSCal_stringToDate()).ToList();
                }
                if (HF_inv_dtInvoice_to.Value.ToInt32() != 0)
                {
                    _list = _list.Where(x => x.inv_dtInvoice <= HF_inv_dtInvoice_to.Value.JSCal_stringToDate()).ToList();
                }
                CURRENT_LIST = _list.OrderByDescending(x => x.inv_dtInvoice).ToList();
            }
            Fill_LV();
            Fill_stats();
        }
        protected void Fill_LV()
        {
            LV.DataSource = CURRENT_LIST;
            LV.DataBind();
        }
        protected void Fill_stats()
        {
            pnl_stats.Visible = true;
            decimal _count = CURRENT_LIST.Count;
            txt_count.Text = _count.ToString();
            decimal _prTotal = CURRENT_LIST.Sum(x => x.pr_total.objToDecimal());
            txt_prTotal.Text = _prTotal.ToString("N2");

            decimal _prTotalMedia = _count != 0 ? _prTotal / _count : 0;
            txt_prTotalMedia.Text = _prTotalMedia.ToString("N2");

        }

    }
}