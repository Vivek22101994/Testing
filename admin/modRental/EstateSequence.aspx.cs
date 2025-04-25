using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace ModRental.admin.modRental
{
    public partial class EstateSequence : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
                fillData();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setSortable", "setSortable();", true);
        }
        protected void fillData()
        {
            LDS.DataBind();
            LV.DataBind();
        }
        protected void saveData()
        {
            List<string> _list = HF_order.Value.splitStringToList("|");
            for (int i = 0; i < _list.Count; i++)
            {
                RNT_TB_ESTATE currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _list[i].ToInt32());
                if (currTBL != null)
                {
                    currTBL.sequence = i + 1;
                }
            }
            DC_RENTAL.SubmitChanges();
            AppSettings._refreshCache_RNT_ESTATEs();
            fillData();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "radAlert", "radalert(\"Ordinamento salvato correttamente.\");", true);
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }
}