using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;
using Telerik.Web.UI;

namespace RentalInRome.admin.modRental
{
    public partial class EstateExtraPackList : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "extras";
        }
        protected dbRntExtrasPackTB currTBL;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                closeDetails(false);
            }
        }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            closeDetails(false);
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Label lbl_id = e.Item.FindControl("lbl_id") as Label;
                if (lbl_id == null) return;
                using (DCmodRental dc = new DCmodRental())
                {
                    currTBL = dc.dbRntExtrasPackTBs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                    List<dbRntExtrasPackLN> currTBLLang = dc.dbRntExtrasPackLNs.Where(x => x.pidPack == lbl_id.Text.ToInt32()).ToList();

                    if (currTBL != null)
                    {
                        dc.Delete(currTBL);
                    }
                    if (currTBLLang != null)
                    {
                        dc.Delete(currTBLLang);
                    }
                   
                    dc.SaveChanges();
                }

            }
            closeDetails(false);

        }
        protected void closeDetails(bool pnlFasciaReload)
        {
            setfilters();
            LDS.Where = ltrLDSfiltter.Text;
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            if (pnlFasciaReload)
                pnlFascia.ResponseScripts.Add(String.Format("$find('{0}').ajaxRequest();", pnlFascia.ClientID));
        }
        protected void setfilters()
        {
            string _filter = "";
            string _sep = "";
            if (txt_flt_title.Text.Trim() != "")
            {
                _filter += _sep + "code.Contains(\"" + txt_flt_title.Text.Trim() + "\")";
                _sep = " and ";
            }
            if (drp_isActive.SelectedValue != "-1")
            {
                _filter += _sep + "isActive = " + drp_isActive.SelectedValue + "";
                _sep = " and ";
            }


            // _filter += _sep + "isActive = 1";
            ltrLDSfiltter.Text = _filter;
        }
        protected void lnk_filter_Click(object sender, EventArgs e)
        {
            closeDetails(false);
        }
    }
}