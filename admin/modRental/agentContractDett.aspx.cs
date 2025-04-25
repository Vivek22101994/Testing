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
    public partial class agentContractDett : adminBasePage
    {
        protected dbRntAgentContractTBL currTbl;
        protected magaRental_DataContext DC_RENTAL;
        protected int IdAgent
        {
            get { return HfIdAgent.Value.ToInt32(); }
            set { HfIdAgent.Value = value.ToString(); }
        }
        protected long IdCurrent
        {
            get { return HfCurrId.Value.ToInt64(); }
            set { HfCurrId.Value = value.ToString(); }
        }
        private List<dbRntAgentContractPricesTBL> TMPcurrPrices;
        private List<dbRntAgentContractPricesTBL> currPrices
        {
            get
            {
                if (TMPcurrPrices == null)
                    if (ViewState["TMPcurrPrices"] != null)
                    {
                        TMPcurrPrices =
                            PConv.DeserArrToList((object[])ViewState["TMPcurrPrices"],
                                                 typeof(dbRntAgentContractPricesTBL)).Cast<dbRntAgentContractPricesTBL>().ToList();
                    }
                    else
                        TMPcurrPrices = new List<dbRntAgentContractPricesTBL>();

                return TMPcurrPrices;
            }
            set
            {
                ViewState["TMPcurrPrices"] = PConv.SerialList(value.Cast<object>().ToList());
                TMPcurrPrices = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                bool canView = true;
                IdAgent = Request.QueryString["idagent"].ToInt32();
                IdCurrent = Request.QueryString["id"].ToInt64();
                using (DCmodRental dc = new DCmodRental())
                {
                    var tmpTbl = dc.dbRntAgentTBLs.SingleOrDefault(x => x.id == IdAgent);
                    if (tmpTbl == null)
                    {
                        CloseRadWindow("");
                        pnlDett.Visible = false;
                        return;
                    }

                    if (UserAuthentication.CURRENT_USER_ROLE != 1)
                    {
                        List<int> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.rnt_canHaveAgent == 1).Select(x => x.id).ToList();
                        if (!_list.Contains(UserAuthentication.CurrentUserID))
                        {
                            canView = false;
                        }
                        else if (tmpTbl.pidReferer != UserAuthentication.CurrentUserID)
                        {
                            canView = false;
                        }
                    }
                }

                if (!canView)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert(\"Non hai permessi per questa pagina. Contattare l'amministratore.\");CloseRadWindow('reload');", true);
                    pnlDett.Visible = false;
                    return;
                }
                drp_flt_pidCity_DataBind();
                drp_flt_pidOwner_DataBind();
                //chkList_estates_DataBind();
                fillData();
            }
        }
        protected void chkList_estates_DataBind()
        {
            var zoneIds = chkList_flt_pidZone.getSelectedValueList().Select(x => (int?)x.ToInt32()).ToList();
            var tmpList = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1
                            && (drp_flt_pidCity.getSelectedValueInt(0) <= 0 || x.pid_city == drp_flt_pidCity.getSelectedValueInt(0))
                            && (zoneIds.Count <= 0 || zoneIds.Contains(x.pid_zone))
                            && (drp_flt_pidOwner.getSelectedValueInt(0) <= 0 || x.pid_owner == drp_flt_pidOwner.getSelectedValueInt(0))
                            && (txt_flt_code.Text.Trim() == "" || x.code.ToLower().Contains(txt_flt_code.Text.Trim().ToLower()))
                            ).OrderBy(x => x.code).ToList();

            using (DCmodRental dc = new DCmodRental())
            {
                var existingEstateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgentContract == IdCurrent && x.pidAgent == IdAgent).Select(x => x.pidEstate).Distinct().ToList();
                if (existingEstateIds != null && existingEstateIds.Count() > 0 && tmpList != null && tmpList.Count() > 0)
                {
                    tmpList = tmpList.Where(x => !existingEstateIds.Contains(x.id)).ToList();
                }
            }

            chkList_estates.DataSource = tmpList;
            chkList_estates.DataTextField = "code";
            chkList_estates.DataValueField = "id";
            chkList_estates.DataBind();
        }
        private void fillData()
        {
            using (DCmodRental dc = new DCmodRental())
            {
                currTbl = dc.dbRntAgentContractTBLs.SingleOrDefault(x => x.id == IdCurrent);
                if (currTbl == null)
                {
                    currTbl = new dbRntAgentContractTBL();
                    HfCurrId.Value = "0";
                    ltrTitle.Text = "Nuovo Contratto per l'Agenzia";
                }
                else
                    ltrTitle.Text = "Contratto #" + currTbl.contractNumber;

                txt_contractNumber.Text = currTbl.contractNumber;
                drp_contractType.setSelectedValue(currTbl.contractType);
                ntxt_commissionAmount.Value = currTbl.commissionAmount.objToDouble();
                rdp_contract_dtStart.SelectedDate = currTbl.dtStart;
                rdp_contract_dtEnd.SelectedDate = currTbl.dtEnd;
                drp_IsSendPrice.setSelectedValue(currTbl.IsSendPrice);

                #region do not set selected value 
                //var tmpEstateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgentContract == IdCurrent && x.pidAgent == IdAgent).Select(x => x.pidEstate.ToString()).Distinct().ToList();
                //chkList_estates.setSelectedValues(tmpEstateIds);  
                #endregion

                var tmpEstateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgentContract == IdCurrent && x.pidAgent == IdAgent).Select(x => x.pidEstate).Distinct().ToList();
                lvExistingApt_DataBind(tmpEstateIds);
                currPrices = dc.dbRntAgentContractPricesTBLs.Where(x => x.pidAgentContract == IdCurrent).ToList();
                if (drp_contractType.SelectedValue == "PPB") lbl_commissionAmount.Text = "Commissione in %";
                if (drp_contractType.SelectedValue == "PPL") lbl_commissionAmount.Text = "Costo per richiesta";
                if (drp_contractType.SelectedValue == "PPS") lbl_commissionAmount.Text = "Costo fisso";                
                fillItems();
            }
        }
        private void saveData()
        {

            using (DCmodRental dc = new DCmodRental())
            {
                currTbl = dc.dbRntAgentContractTBLs.SingleOrDefault(x => x.id == IdCurrent);
                if (currTbl == null)
                {
                    currTbl = new dbRntAgentContractTBL();
                    dc.Add(currTbl);
                }
                currTbl.pidAgent = IdAgent;
                currTbl.contractNumber = txt_contractNumber.Text;
                currTbl.contractType = drp_contractType.SelectedValue;
                currTbl.commissionAmount = ntxt_commissionAmount.Value.objToDecimal();
                currTbl.dtStart = rdp_contract_dtStart.SelectedDate;
                currTbl.dtEnd = rdp_contract_dtEnd.SelectedDate;
                currTbl.IsSendPrice = drp_IsSendPrice.getSelectedValueInt();

                dc.SaveChanges();
                IdCurrent = currTbl.id;

                //dc.Delete(dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgentContract == IdCurrent));
                //dc.SaveChanges();
                var lstEstates = chkList_estates.getSelectedValueList();
                foreach (string estate in lstEstates)
                {
                    try
                    {
                        dbRntEstateAgentContractRL tmp = new dbRntEstateAgentContractRL();
                        tmp.pidEstate = estate.ToInt32();
                        tmp.pidAgent = IdAgent;
                        tmp.pidAgentContract = IdCurrent;
                        dc.Add(tmp);
                        dc.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.addLog("", "SaveEstateAgentContractRL", ex.ToString());
                    }               
                }

                dc.Delete(dc.dbRntAgentContractPricesTBLs.Where(x => x.pidAgentContract == IdCurrent));
                dc.SaveChanges();
                saveItems();
                foreach (var tmp in currPrices)
                {
                    tmp.pidAgentContract = IdCurrent;
                    dc.Add(tmp);
                    dc.SaveChanges();
                }
                fillData();
                chkList_estates_DataBind();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);

            }

        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("agentDett.aspx?id=" + IdAgent);
        }
        protected void lnkAddNewContract_Click(object sender, EventArgs e)
        {
            HfCurrId.Value = "";
            fillData();
        }
        protected void fillItems()
        {
            LV.DataSource = currPrices.OrderByDescending(x => x.dtStart);
            LV.DataBind();
            foreach (ListViewDataItem _item in LV.Items)
            {
                RadDatePicker rdp_dtStart = _item.FindControl("rdp_dtStart") as RadDatePicker;
                RadDatePicker rdp_dtEnd = _item.FindControl("rdp_dtEnd") as RadDatePicker;
                RadNumericTextBox ntxt_commissionAmount = _item.FindControl("ntxt_commissionAmount") as RadNumericTextBox;
                Label lbl_id = _item.FindControl("lbl_id") as Label;
                dbRntAgentContractPricesTBL _new = currPrices.SingleOrDefault(x => x.uid == new Guid(lbl_id.Text));
                if (_new == null)
                {
                    _new = new dbRntAgentContractPricesTBL();
                }

                rdp_dtEnd.SelectedDate = _new.dtEnd;
                rdp_dtStart.SelectedDate = _new.dtStart;
                ntxt_commissionAmount.Value = _new.commissionAmount.objToDouble();
            }
        }
        protected string saveItems()
        {
            string _error = "";
            decimal cashTaxFree = 0;
            decimal cashTaxAmount = 0;
            decimal cashTotalAmount = 0;
            List<dbRntAgentContractPricesTBL> _list = currPrices;
            foreach (ListViewDataItem _item in LV.Items)
            {
                RadDatePicker rdp_dtStart = _item.FindControl("rdp_dtStart") as RadDatePicker;
                RadDatePicker rdp_dtEnd = _item.FindControl("rdp_dtEnd") as RadDatePicker;
                RadNumericTextBox ntxt_commissionAmount = _item.FindControl("ntxt_commissionAmount") as RadNumericTextBox;
                Label lbl_id = _item.FindControl("lbl_id") as Label;

                dbRntAgentContractPricesTBL _new = _list.SingleOrDefault(x => x.uid == new Guid(lbl_id.Text));
                if (_new == null)
                {
                    _new = new dbRntAgentContractPricesTBL();
                    _new.uid = Guid.NewGuid();
                    _list.Add(_new);
                }
                _new.pidAgentContract = 0;
                _new.dtEnd = rdp_dtEnd.SelectedDate.Value;
                _new.dtStart = rdp_dtStart.SelectedDate.Value;
                _new.commissionAmount = ntxt_commissionAmount.Value.objToDecimal();
            }
            if (_list.Count == 0)
                _error += "- inserire Almeno 1 Oggetto della fattura<br/>";
            currPrices = _list;
            fillItems();
            return _error;
        }
        protected void lnk_add_new_Click(object sender, EventArgs e)
        {
            saveItems();
            List<dbRntAgentContractPricesTBL> _list = currPrices;
            dbRntAgentContractPricesTBL _new = new dbRntAgentContractPricesTBL();
            _new.uid = Guid.NewGuid();
            _new.pidAgentContract = 0;
            _new.dtStart = DateTime.Now;
            _new.dtEnd = DateTime.Now.AddDays(1);
            _new.commissionAmount = 0;
            _list.Add(_new);
            currPrices = _list;
            fillItems();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                saveItems();
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                List<dbRntAgentContractPricesTBL> _list = currPrices;
                dbRntAgentContractPricesTBL _del = _list.SingleOrDefault(x => x.uid == new Guid(lbl_id.Text));
                if (_del != null)
                    _list.Remove(_del);
                currPrices = _list;
                fillItems();
            }
        }

        private void drp_flt_pidCity_DataBind()
        {
            List<LOC_VIEW_CITY> list = maga_DataContext.DC_LOCATION.LOC_VIEW_CITies.Where(x => x.is_active == 1 && x.pid_lang == 1).ToList();
            drp_flt_pidCity.DataSource = list;
            drp_flt_pidCity.DataTextField = "title";
            drp_flt_pidCity.DataValueField = "id";
            drp_flt_pidCity.DataBind();
            drp_flt_pidCity.Items.Insert(0, new ListItem("- tutti -", "0"));
        }

        protected void chkList_flt_pidZone_DataBind()
        {            
            var zoneIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1 && x.pid_zone.HasValue).Select(x => x.pid_zone.Value).Distinct().ToList();
            chkList_flt_pidZone.DataSource = maga_DataContext.DC_LOCATION.LOC_VIEW_ZONEs.Where(x => x.is_active == 1 && x.pid_lang == App.DefLangID && zoneIds.Contains(x.id) && (drp_flt_pidCity.getSelectedValueInt(0) == 0 || x.pid_city == drp_flt_pidCity.getSelectedValueInt(0))).OrderBy(x => x.title);
            chkList_flt_pidZone.DataTextField = "title";
            chkList_flt_pidZone.DataValueField = "id";
            chkList_flt_pidZone.DataBind();
        }

        private void drp_flt_pidOwner_DataBind()
        {
            using (magaUser_DataContext dc = maga_DataContext.DC_USER)
            {
                var ownerIds = AppSettings.RNT_TB_ESTATE.Where(x => x.is_deleted != 1 && x.is_active == 1 && x.pid_owner.HasValue).Select(x => x.pid_owner.Value).Distinct().ToList();
                drp_flt_pidOwner.DataSource = dc.USR_TBL_OWNER.Where(x => x.is_active == 1 && ownerIds.Contains(x.id)).OrderBy(x => x.name_full);
                drp_flt_pidOwner.DataTextField = "name_full";
                drp_flt_pidOwner.DataValueField = "id";
                drp_flt_pidOwner.DataBind();
                drp_flt_pidOwner.Items.Insert(0, new ListItem("- tutti -", "-1"));
            }
        }

        protected void drp_flt_pidCity_SelectedIndexChanged(object sender, EventArgs e)
        {
             chkList_flt_pidZone_DataBind();
        }
   
        private void lvExistingApt_DataBind(List<int> tmpEstateIds)
        {
            lvExistingApt.DataSource = tmpEstateIds;
            lvExistingApt.DataBind();
        }

        protected void imgDel_Click(object sender, ImageClickEventArgs e)
        {
            // var tmpEstateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgentContract == IdCurrent && x.pidAgent == IdAgent).Select(x => x.pidEstate).Distinct().ToList();
            ImageButton imgDel = sender as ImageButton;
            using (DCmodRental dc = new DCmodRental())
            {
                var curr = dc.dbRntEstateAgentContractRLs.FirstOrDefault(x => x.pidAgentContract == IdCurrent && x.pidAgent == IdAgent && x.pidEstate == imgDel.CommandArgument.objToInt32());
                if (curr != null)
                {
                    dc.Delete(curr);
                    dc.SaveChanges();
                }
                var tmpEstateIds = dc.dbRntEstateAgentContractRLs.Where(x => x.pidAgentContract == IdCurrent && x.pidAgent == IdAgent).Select(x => x.pidEstate).Distinct().ToList();
                lvExistingApt_DataBind(tmpEstateIds);
            }
        }

        protected void lnk_chkListSelectAll_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (sender as LinkButton);
            string arg = lnk.CommandArgument;
            if (arg.Contains("pidZone"))
                foreach (ListItem item in chkList_flt_pidZone.Items)
                    item.Selected = !arg.Contains("deselect");
            if (arg.Contains("pidEstate"))
                foreach (ListItem item in chkList_estates.Items)
                    item.Selected = !arg.Contains("deselect");
        }

        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            chkList_estates_DataBind();
        }
    }
}

