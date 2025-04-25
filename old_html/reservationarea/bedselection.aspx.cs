using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.reservationarea
{
    public partial class bedselection : basePage
    {
        [Serializable]
        public class BedsConfig
        {
            public int bedSingle { get; set; }
            public int bedDouble { get; set; }
            public int bedDoubleD { get; set; }
            public int bedDoubleDConfig { get; set; }
            public int bedDouble2level { get; set; }
            public string bedDouble2levelConfig { get; set; }
            public int bedSofaSingle { get; set; }
            public int bedSofaDouble { get; set; }
            public int persMin { get; set; }
            public int persMax { get; set; }
            public BedsConfig() { }
        }
        private BedsConfig tmpBedsConfig;
        public BedsConfig currBedsConfig
        {
            get
            {
                if (tmpBedsConfig == null)
                    tmpBedsConfig = (BedsConfig)ViewState["currBedsConfig"];
                return tmpBedsConfig ?? new BedsConfig();
            }
            set { tmpBedsConfig = value; ViewState["currBedsConfig"] = value; }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            adminAccess = true;
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_RESERVATION tmpReservationTBL;
        public RNT_TBL_RESERVATION currReservationTBL
        {
            get
            {
                if (tmpReservationTBL == null)
                {
                    DC_RENTAL = maga_DataContext.DC_RENTAL;
                    tmpReservationTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
                }
                return tmpReservationTBL ?? new RNT_TBL_RESERVATION();
            }
        }
        public int IdEstate
        {
            get { return HF_IdEstate.Value.ToInt32(); }
            set { HF_IdEstate.Value = value.ToString(); }
        }
        private RNT_TB_ESTATE tmpEstateTB;
        public RNT_TB_ESTATE currEstateTB
        {
            get
            {
                if (tmpEstateTB == null)
                    tmpEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                return tmpEstateTB ?? new RNT_TB_ESTATE();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                fillData();
            }
        }
        protected void fillData()
        {
            tmpReservationTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (tmpReservationTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            tmpEstateTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == tmpReservationTBL.pid_estate);
            if (tmpEstateTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            IdEstate = tmpEstateTB.id;
            HF_id.Value = tmpReservationTBL.id.ToString();
            tmpBedsConfig = new BedsConfig();
            tmpBedsConfig.bedSingle = tmpReservationTBL.bedSingle.objToInt32();
            tmpBedsConfig.bedDouble = tmpReservationTBL.bedDouble.objToInt32();
            tmpBedsConfig.bedDoubleD = tmpReservationTBL.bedDoubleD.objToInt32();
            tmpBedsConfig.bedDoubleDConfig = tmpReservationTBL.bedDoubleDConfig.objToInt32();
            tmpBedsConfig.bedDouble2level = tmpReservationTBL.bedDouble2level.objToInt32();
            tmpBedsConfig.bedDouble2levelConfig = tmpReservationTBL.bedDouble2levelConfig;
            tmpBedsConfig.bedSofaSingle = tmpReservationTBL.bedSofaSingle.objToInt32();
            tmpBedsConfig.bedSofaDouble = tmpReservationTBL.bedSofaDouble.objToInt32();
            tmpBedsConfig.persMin = 0
                + tmpBedsConfig.bedSingle
                + tmpBedsConfig.bedDouble
                + (tmpBedsConfig.bedDoubleD - tmpBedsConfig.bedDoubleDConfig)
                + tmpBedsConfig.bedDoubleDConfig * 2
                + tmpBedsConfig.bedDouble2level
                + tmpBedsConfig.bedSofaSingle
                + tmpBedsConfig.bedSofaDouble;
            tmpBedsConfig.persMax = 0
                + tmpBedsConfig.bedSingle
                + tmpBedsConfig.bedDouble * 2
                + tmpBedsConfig.bedDoubleD * 2
                + tmpBedsConfig.bedDouble2level * 2
                + tmpBedsConfig.bedSofaSingle
                + tmpBedsConfig.bedSofaDouble * 2;
            currBedsConfig = tmpBedsConfig;

            pnl_bedDouble.Visible = currEstateTB.num_bed_double.objToInt32() > 0;
            drpBeds_DataBind(ref drp_bedDouble, currEstateTB.num_bed_double.objToInt32());
            drp_bedDouble.setSelectedValue(tmpBedsConfig.bedDouble);

            pnl_bedDoubleD.Visible = currEstateTB.num_bed_double_divisible.objToInt32() > 0;
            drpBeds_DataBind(ref drp_bedDoubleD, currEstateTB.num_bed_double_divisible.objToInt32());
            drp_bedDoubleD.setSelectedValue(tmpBedsConfig.bedDoubleD);
            drpBeds_DataBind(ref drp_bedDoubleDConfig, tmpBedsConfig.bedDoubleD);
            pnl_bedDoubleDConfig.Visible = drp_bedDoubleDConfig.Items.Count > 1;
            drp_bedDoubleDConfig.setSelectedValue(tmpBedsConfig.bedDoubleDConfig);

            pnl_bedSingle.Visible = currEstateTB.num_bed_single.objToInt32() > 0;
            drpBeds_DataBind(ref drp_bedSingle, currEstateTB.num_bed_single.objToInt32());
            drp_bedSingle.setSelectedValue(tmpBedsConfig.bedSingle);

            pnl_bedSofaDouble.Visible = currEstateTB.num_sofa_double.objToInt32() > 0;
            drpBeds_DataBind(ref drp_bedSofaDouble, currEstateTB.num_sofa_double.objToInt32());
            drp_bedSofaDouble.setSelectedValue(tmpBedsConfig.bedSofaDouble);

            pnl_bedSofaSingle.Visible = currEstateTB.num_sofa_single.objToInt32() > 0;
            drpBeds_DataBind(ref drp_bedSofaSingle, currEstateTB.num_sofa_single.objToInt32());
            drp_bedSofaSingle.setSelectedValue(tmpBedsConfig.bedSofaSingle);

            pnl_bedDouble2level.Visible = currEstateTB.num_bed_double_2level.objToInt32() > 0;
            drpBeds_DataBind(ref drp_bedDouble2level, currEstateTB.num_bed_double_2level.objToInt32());
            drp_bedDouble2level.setSelectedValue(tmpBedsConfig.bedDouble2level);

        }
        protected void saveData()
        {
            tmpReservationTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (tmpReservationTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            tmpReservationTBL.bedSingle = currBedsConfig.bedSingle;
            tmpReservationTBL.bedDouble = currBedsConfig.bedDouble;
            tmpReservationTBL.bedDoubleD = currBedsConfig.bedDoubleD;
            tmpReservationTBL.bedDoubleDConfig = currBedsConfig.bedDoubleDConfig;
            tmpReservationTBL.bedDouble2level = currBedsConfig.bedDouble2level;
            tmpReservationTBL.bedDouble2levelConfig = currBedsConfig.bedDouble2levelConfig;
            tmpReservationTBL.bedSofaSingle = currBedsConfig.bedSofaSingle;
            tmpReservationTBL.bedSofaDouble = currBedsConfig.bedSofaDouble;
            DC_RENTAL.SubmitChanges();
            rntUtils.rntReservation_onChange(tmpReservationTBL);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"Data was successfully saved. \");", true);
            fillData();
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            int numPerson = currReservationTBL.num_adult.objToInt32() + currReservationTBL.num_child_over.objToInt32();
            if (numPerson > currBedsConfig.persMax)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"Error on saving data.<br/>You have not selected enough number of beds, please select some more.\");", true);
                return;
            }
            if (numPerson < currBedsConfig.persMin)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"Error on saving data.<br/>You have selected too many beds, please remove some of them.\");", true);
                return;
            }
            saveData();
        }
        protected void drpBeds_DataBind(ref DropDownList drp, int max)
        {
            drp.Items.Clear();
            drp.Items.Add(new ListItem("- - -", "0"));
            for (int i = 1; i <= max; i++)
            {
                drp.Items.Add(new ListItem("" + i + " " + CurrentSource.getSysLangValue(i == 1 ? "lblBed" : "lblBeds"), "" + i));
            }
        }
        protected void drpBeds_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmpBedsConfig = currBedsConfig;
            tmpBedsConfig.bedSingle = drp_bedSingle.getSelectedValueInt();
            tmpBedsConfig.bedDouble = drp_bedDouble.getSelectedValueInt();
            tmpBedsConfig.bedDoubleD = drp_bedDoubleD.getSelectedValueInt();
            tmpBedsConfig.bedDoubleDConfig = drp_bedDoubleDConfig.getSelectedValueInt();
            drpBeds_DataBind(ref drp_bedDoubleDConfig, tmpBedsConfig.bedDoubleD);
            pnl_bedDoubleDConfig.Visible = drp_bedDoubleDConfig.Items.Count > 1;
            drp_bedDoubleDConfig.setSelectedValue(tmpBedsConfig.bedDoubleDConfig);
            tmpBedsConfig.bedDouble2level = drp_bedDouble2level.getSelectedValueInt();
            tmpBedsConfig.bedDouble2levelConfig = "";
            tmpBedsConfig.bedSofaSingle = drp_bedSofaSingle.getSelectedValueInt();
            tmpBedsConfig.bedSofaDouble = drp_bedSofaDouble.getSelectedValueInt();

            tmpBedsConfig.persMin = 0 
                + tmpBedsConfig.bedSingle
                + tmpBedsConfig.bedDouble
                + (tmpBedsConfig.bedDoubleD - tmpBedsConfig.bedDoubleDConfig)
                + tmpBedsConfig.bedDoubleDConfig * 2
                + tmpBedsConfig.bedDouble2level
                + tmpBedsConfig.bedSofaSingle
                + tmpBedsConfig.bedSofaDouble;
            tmpBedsConfig.persMax = 0
                + tmpBedsConfig.bedSingle
                + tmpBedsConfig.bedDouble * 2
                + tmpBedsConfig.bedDoubleD * 2
                + tmpBedsConfig.bedDouble2level * 2
                + tmpBedsConfig.bedSofaSingle
                + tmpBedsConfig.bedSofaDouble * 2;
            currBedsConfig = tmpBedsConfig;
        }
        protected void LV_invoice_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            Label lbl_uid = e.Item.FindControl("lbl_uid") as Label;
            HyperLink HL_pdf = e.Item.FindControl("HL_pdf") as HyperLink;
            if (lbl_uid == null || lbl_id == null || HL_pdf == null) return;

            HL_pdf.Enabled = false;
            HL_pdf.CssClass = "btnDownload inattivo";
            if (tmpReservationTBL.cl_isCompleted != 1) return;
            string url_voucher = CurrentAppSettings.HOST + CurrentAppSettings.ROOT_PATH + "pdfgenerator/pdf_rnt_reservation_invoice.aspx?uid=" + lbl_uid.Text;
            string filename = "RiR-reservation_invoice-code_" + tmpReservationTBL.code + ".pdf";
            HL_pdf.Enabled = true;
            HL_pdf.CssClass = "btnDownload";
            HL_pdf.NavigateUrl = CurrentAppSettings.ROOT_PATH + "pdfgenerator/generator.aspx?url=" + url_voucher.urlEncode() + "&filename=" + filename.urlEncode();


        }

    }
}