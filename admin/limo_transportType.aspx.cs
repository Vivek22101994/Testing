using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class limo_transportType : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "limo_transportType";
        }
        protected magaLimo_DataContext DC_LIMO;

        protected LIMO_LK_TRANSPORTTYPE _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_LIMO = maga_DataContext.DC_LIMO;
            if (!IsPostBack)
            {
            }
        }

        protected void LV_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            var lbl_id = (Label)LV.Items[e.NewSelectedIndex].FindControl("lbl_id");
            HF_id.Value = lbl_id.Text;
            FillControls();
        }

        private void FillControls()
        {
            _currTBL = DC_LIMO.LIMO_LK_TRANSPORTTYPE.SingleOrDefault(x => x.code == HF_id.Value);
            if (_currTBL == null)
            {
                return;
            }


            HF_lang.Value = "1";
            txt_title.Text = _currTBL.title;
            chk_is_active.Checked = _currTBL.isActive == 1;
            pnlContent.Visible = true;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scrolTo", "$.scrollTo($(\"#" + pnlContent.ClientID + "\"), 500);", true);
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            _currTBL = DC_LIMO.LIMO_LK_TRANSPORTTYPE.SingleOrDefault(x => x.code == HF_id.Value);
            if (_currTBL==null)
            {
                return;
            }
            _currTBL.title = txt_title.Text;
            _currTBL.isActive = chk_is_active.Checked ? 1 : 0;
            DC_LIMO.SubmitChanges();
            pnlContent.Visible = false;
            LDS.DataBind();
            LV.DataBind();
            LV.SelectedIndex = -1;
        }

        protected void lnk_nuovo_Click(object sender, EventArgs e)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            HF_id.Value = "0";
            FillControls();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
        protected void LV_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            pnlContent.Visible = false;
        }
    }
}
