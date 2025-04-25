using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class ChnlHomeAwayAcceptedPaymentForms : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            //manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(manager_AjaxRequest);

            if (!IsPostBack)
            {
                fillData();
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            fillData();
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "get")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                
            }
        }
        protected void lnk_flt_Click(object sender, EventArgs e)
        {
            fillData();
        }
        private void fillData()
        {
            using (var DC_RENTAL = maga_DataContext.DC_RENTAL)
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            using (DCmodRental dc = new DCmodRental())
            {
                var list = dcChnl.dbRntChnlHomeAwayAcceptedPaymentFormTBLs.OrderBy(x => x.paymentFormType).ThenBy(x => x.cardCode).ThenBy(x => x.cardType).ToList();
                LV.DataSource = list.ToList();
                LV.DataBind();
                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_paymentFormType = item.FindControl("lbl_paymentFormType") as Label;
                    Label lbl_cardCode = item.FindControl("lbl_cardCode") as Label;
                    Label lbl_cardType = item.FindControl("lbl_cardType") as Label;
                    Label lbl_isActive = item.FindControl("lbl_isActive") as Label;
                    CheckBox chk = item.FindControl("chk") as CheckBox;

                    chk.Checked = lbl_isActive.Text.ToInt32() == 1;
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            using (DCchnlHomeAway dcChnl = new DCchnlHomeAway())
            {
                foreach (ListViewDataItem item in LV.Items)
                {
                    Label lbl_paymentFormType = item.FindControl("lbl_paymentFormType") as Label;
                    Label lbl_cardCode = item.FindControl("lbl_cardCode") as Label;
                    Label lbl_cardType = item.FindControl("lbl_cardType") as Label;
                    Label lbl_isActive = item.FindControl("lbl_isActive") as Label;
                    CheckBox chk = item.FindControl("chk") as CheckBox;

                    var featureValuesTbl = dcChnl.dbRntChnlHomeAwayAcceptedPaymentFormTBLs.SingleOrDefault(x => x.cardType == lbl_cardType.Text && x.cardCode == lbl_cardCode.Text && x.paymentFormType == lbl_paymentFormType.Text);
                    if (featureValuesTbl != null)
                    {
                        featureValuesTbl.isActive = chk.Checked ? 1 : 0;
                        dcChnl.SaveChanges();
                    }
                }
            }
            fillData();
        }
    }

}