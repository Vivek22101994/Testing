using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlAtraveo_fees : adminBasePage
    {
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }
        protected string CurrUid
        {
            get
            {
                return HF_uid.Value;
            }
            set
            {
                HF_uid.Value = value.ToString();
            }
        }
        protected List<string> propertyTypes = new List<string>() { "house", "condo", "cabin", "villa", "resort", "b&b", "chalet", "castle", "yacht", "cottage", "houseboat", "estate", "farmhouse", "other" };
        protected List<string> unitSizeUnits = new List<string>() { "feet", "meters" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();

                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ltr_apartment.Text = currEstate.code;
                ucNav.IdEstate = IdEstate;
                fillList();
                fillUnit(ref drpUnit);
                fillCode(ref Code);
            }
        }
        List<dbRntChnlAtraveoLkFeeValuesTBL> feeValues;
        protected void fillCode(ref DropDownList drp)
        {
            if (feeValues == null)
                using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
                    feeValues = dcChnl.dbRntChnlAtraveoLkFeeValuesTBLs.ToList();
            drp.DataSource = feeValues.OrderBy(x => x.title).ToList();
            drp.DataTextField = "title";
            drp.DataValueField = "code";
            drp.DataBind();
        }
        List<dbRntChnlAtraveoLkFeeUnitsTBL> feeUnits;
        protected void fillUnit(ref DropDownList drp)
        {
            if (feeUnits == null)
                using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
                    feeUnits = dcChnl.dbRntChnlAtraveoLkFeeUnitsTBLs.ToList();
            drp.DataSource = feeUnits.OrderBy(x => x.title).ToList();
            drp.DataTextField = "title";
            drp.DataValueField = "code";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("per stay", ""));
        }
        public void fillList()
        {
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {
                var currList = dcChnl.dbRntChnlAtraveoEstateFeeTBs.Where(x => x.pidEstate == IdEstate).OrderBy(x => x.Code).ThenBy(x=>x.Cost).ToList();
                Lv.DataSource = currList;
                Lv.DataBind();
                int roomCount = 0;
                foreach (ListViewDataItem item in Lv.Items)
                {
                    roomCount++;
                    var lbl_id = item.FindControl("lbl_id") as Label;

                    var this_Code = item.FindControl("Code") as DropDownList;
                    var this_Unit = item.FindControl("drpUnit") as DropDownList;
                    var this_CostType = item.FindControl("CostType") as DropDownList;
                    var this_Cost = item.FindControl("Cost") as RadNumericTextBox;
                    var this_IntervalType = item.FindControl("IntervalType") as DropDownList;
                    var this_MandatoryCode = item.FindControl("MandatoryCode") as DropDownList;
                    var this_LocationOrder = item.FindControl("LocationOrder") as DropDownList;

                    var lnkEdit = item.FindControl("lnkEdit") as LinkButton;
                    var lnkSave = item.FindControl("lnkSave") as LinkButton;
                    var lnkCancel = item.FindControl("lnkCancel") as LinkButton;
                    var lnkDel = item.FindControl("lnkDel") as LinkButton;

                    var ltrNumber = item.FindControl("ltrNumber") as Literal;
                    ltrNumber.Text = roomCount + "";

                    var currTbl = currList.SingleOrDefault(x => x.uid.ToString().ToLower() == lbl_id.Text.ToLower());
                    if (currTbl == null)
                    {
                        item.Visible = false;
                        continue;
                    }
                    fillCode(ref this_Code);
                    this_Code.setSelectedValue(currTbl.Code);
                    fillUnit(ref this_Unit);
                    this_Unit.setSelectedValue(currTbl.Unit);
                    this_CostType.setSelectedValue(currTbl.CostType);
                    this_Cost.Value = currTbl.Cost.objToDouble();
                    this_IntervalType.setSelectedValue(currTbl.IntervalType);
                    this_MandatoryCode.setSelectedValue(currTbl.MandatoryCode);
                    this_LocationOrder.setSelectedValue(currTbl.LocationOrder);

                    this_Cost.ReadOnly = CurrUid.ToLower() != currTbl.uid.ToString().ToLower();
                    this_Code.Enabled =
                    this_Unit.Enabled =
                    this_CostType.Enabled =
                    this_IntervalType.Enabled =
                    this_MandatoryCode.Enabled =
                    this_LocationOrder.Enabled = CurrUid.ToLower() == currTbl.uid.ToString().ToLower();

                    lnkEdit.Visible = CurrUid.ToLower() != currTbl.uid.ToString().ToLower();
                    lnkSave.Visible = CurrUid.ToLower() == currTbl.uid.ToString().ToLower();
                    lnkCancel.Visible = CurrUid.ToLower() == currTbl.uid.ToString().ToLower();
                }
            }
        }

        protected void LvItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (lbl_id == null) return;
            if (e.CommandName == "del")
            {
                using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
                {
                    var currTbl = dcChnl.dbRntChnlAtraveoEstateFeeTBs.SingleOrDefault(x => x.uid.ToString().ToLower() == lbl_id.Text.ToLower());
                    if (currTbl != null)
                    {
                        dcChnl.Delete(currTbl);
                        dcChnl.SaveChanges();
                    }
                }
                fillList();
            }
            if (e.CommandName == "mod")
            {
                CurrUid = lbl_id.Text.ToLower();
                fillList();
            }
            if (e.CommandName == "canc")
            {
                CurrUid = "";
                fillList();
            }
            if (e.CommandName == "sav")
            {
                var this_Code = e.Item.FindControl("Code") as DropDownList;
                var this_Unit = e.Item.FindControl("drpUnit") as DropDownList;
                var this_CostType = e.Item.FindControl("CostType") as DropDownList;
                var this_Cost = e.Item.FindControl("Cost") as RadNumericTextBox;
                var this_IntervalType = e.Item.FindControl("IntervalType") as DropDownList;
                var this_MandatoryCode = e.Item.FindControl("MandatoryCode") as DropDownList;
                var this_LocationOrder = e.Item.FindControl("LocationOrder") as DropDownList;

                using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
                {
                    var currTbl = dcChnl.dbRntChnlAtraveoEstateFeeTBs.SingleOrDefault(x => x.uid.ToString().ToLower() == lbl_id.Text.ToLower());
                    if (currTbl == null)
                    {
                        fillList();
                        return;
                    }
                    currTbl.Code = this_Code.SelectedValue;
                    currTbl.Unit = this_Unit.SelectedValue;
                    currTbl.CostType = this_CostType.SelectedValue;
                    currTbl.Cost = this_Cost.Value.objToDecimal();
                    currTbl.IntervalType = this_IntervalType.SelectedValue;
                    currTbl.MandatoryCode = this_MandatoryCode.SelectedValue.ToInt32();
                    currTbl.LocationOrder = this_LocationOrder.SelectedValue;
                    dcChnl.SaveChanges();
                }
                CurrUid = "";
                fillList();
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {
                var currTbl = new dbRntChnlAtraveoEstateFeeTB();
                currTbl.uid = Guid.NewGuid();
                currTbl.pidEstate = IdEstate;

                currTbl.Code = Code.SelectedValue;
                currTbl.Unit = drpUnit.SelectedValue;
                currTbl.CostType = CostType.SelectedValue;
                currTbl.Cost = Cost.Value.objToInt32();
                currTbl.IntervalType = IntervalType.SelectedValue;
                currTbl.MandatoryCode = MandatoryCode.SelectedValue.ToInt32();
                currTbl.LocationOrder = LocationOrder.SelectedValue;
                dcChnl.Add(currTbl);
                dcChnl.SaveChanges();
                Cost.Value = 0;
                IntervalType.Text = "";
                MandatoryCode.Text = "";
            }
            CurrUid = "";
            fillList();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Cost.Value = 0;
        }

    }
}