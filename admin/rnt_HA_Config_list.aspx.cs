using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class rnt_HA_Config_list : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;

        protected RNT_TB_HACONFIG _currTBL;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
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
            _currTBL = new RNT_TB_HACONFIG();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_RENTAL.RNT_TB_HACONFIG.SingleOrDefault(item => item.id == id);
                    txt_title.Text = _currTBL.inner_notes;
                }
            }

           
            pnlContent.Visible = true;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scrolTo", "$.scrollTo($(\"#" + pnlContent.ClientID + "\"), 500);", true);
        }

        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            _currTBL = new RNT_TB_HACONFIG();
            if (HF_id.Value != "0")
            {
                int id;
                if (int.TryParse(HF_id.Value, out id))
                {
                    _currTBL = DC_RENTAL.RNT_TB_HACONFIG.SingleOrDefault(item => item.id == id);
                    _currTBL.inner_notes = txt_title.Text;
                }
            }
            else
            {
                _currTBL.inner_notes = txt_title.Text;
                DC_RENTAL.RNT_TB_HACONFIG.InsertOnSubmit(_currTBL);
            }
            DC_RENTAL.SubmitChanges();
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