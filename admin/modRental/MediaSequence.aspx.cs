using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace ModRental.admin.modRental
{
    public partial class MediaSequence : adminBasePage
    {
        protected magaRental_DataContext DC_RENTAL;
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
                fillData();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                int id = Request.QueryString["id"].ToInt32();
                RNT_VIEW_ESTATE _est = DC_RENTAL.RNT_VIEW_ESTATEs.SingleOrDefault(x => x.id == id && x.pid_lang == 1);
                if (_est != null)
                {
                    ltr_apartment.Text = _est.code + " / " + "rif. " + _est.id;
                    IdEstate = _est.id;
                }
                else
                {
                    CloseRadWindow("");
                    return;
                }
            }
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
                var currTBL = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == _list[i].ToInt32());
                if (currTBL != null)
                {
                    currTBL.sequence = i + 1;
                }
            }
            DC_RENTAL.SubmitChanges();
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