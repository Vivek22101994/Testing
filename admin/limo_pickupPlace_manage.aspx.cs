using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class limo_pickupPlace_manage : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "limo_pickupPlace";
        }
        protected magaLimo_DataContext DC_LIMO;

        protected LIMO_TB_PICKUP_PLACE _currTBL;
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
            _currTBL = new LIMO_TB_PICKUP_PLACE();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_LIMO.LIMO_TB_PICKUP_PLACE.SingleOrDefault(item => item.id == id);
                }
            }

            pnlAir.Visible = _currTBL.type == "air";
            HF_lang.Value = "1";
            txt_title.Text = _currTBL.title;
            chk_is_active.Checked = _currTBL.isActive == 1;
            txt_outTime1.Text = _currTBL.outTime1.objToInt32().ToString();
            txt_outTime2.Text = _currTBL.outTime2.objToInt32().ToString();
            pnlContent.Visible = true;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scrolTo", "$.scrollTo($(\"#" + pnlContent.ClientID + "\"), 500);", true);
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            _currTBL = new LIMO_TB_PICKUP_PLACE();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_LIMO.LIMO_TB_PICKUP_PLACE.SingleOrDefault(item => item.id == id);
                }
            }
            else
            {
                DC_LIMO.LIMO_TB_PICKUP_PLACE.InsertOnSubmit(_currTBL);
            }
            _currTBL.title = txt_title.Text;
            _currTBL.isActive = chk_is_active.Checked ? 1 : 0;
            _currTBL.outTime1 = txt_outTime1.Text.ToInt32();
            _currTBL.outTime2 = txt_outTime2.Text.ToInt32();
            DC_LIMO.SubmitChanges();
            limoProps.LIMO_TB_PICKUP_PLACE = DC_LIMO.LIMO_TB_PICKUP_PLACE.ToList();
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
