using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_usr_lang_admin : System.Web.UI.UserControl
    {
        public int LangID
        {
            get
            {
                int _id;
                if (int.TryParse(HF_id.Value, out _id))
                    return _id;
                return 0;
            }
            set
            {
                HF_id.Value = value.ToString();
                DC_USER = maga_DataContext.DC_USER;
                DC_CONTENT = maga_DataContext.DC_CONTENT;
                FillControls();
            }
        }

        protected CONT_TBL_LANG _currTBL;
        protected magaUser_DataContext DC_USER;
        protected magaContent_DataContext DC_CONTENT;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack)
            {
            }
        }


        private void FillControls()
        {
            if (LangID != 0)
                _currTBL = DC_CONTENT.CONT_TBL_LANGs.SingleOrDefault(x => x.id == LangID);
            else
                return;
            // Fatturazione
            ltr_title.Text = "Gestori delle prenotazioni in lingua: " + _currTBL.title;
        }

        protected void FillDataFromControls()
        {
            if (LangID != 0)
                _currTBL = DC_CONTENT.CONT_TBL_LANGs.SingleOrDefault(x => x.id == LangID);
            else
                return;

            DC_USER.SubmitChanges();
        }

        protected void DisableControls()
        {
            pnl_view.Visible = true;
            pnl_modify.Visible = false;

            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
            lnk_modify.Visible = true;
        }
        protected void EnableControls()
        {
            pnl_view.Visible = false;
            pnl_modify.Visible = true;

            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
            lnk_modify.Visible = false;
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            FillDataFromControls();
            DisableControls();
        }
        protected void lnk_modify_Click(object sender, EventArgs e)
        {
            EnableControls();
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            FillControls();
        }
    }
}