using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_usr_rl_country_lang : System.Web.UI.UserControl
    {
        public int IdCountry
        {
            get
            {
                int _id;
                if (int.TryParse(HF_pid_country.Value, out _id))
                    return _id;
                return -1;
            }
            set
            {
                HF_pid_country.Value = value.ToString();
            }
        }
        public int IdLang
        {
            get
            {
                int _id;
                if (int.TryParse(HF_pid_lang.Value, out _id))
                    return _id;
                return -1;
            }
            set
            {
                HF_pid_lang.Value = value.ToString();
            }
        }
        public int IdAdmin
        {
            get
            {
                int _id;
                if (int.TryParse(HF_pid_admin.Value, out _id))
                    return _id;
                return -1;
            }
            set
            {
                HF_pid_admin.Value = value.ToString();
            }
        }
        public bool isEdit
        {
            get
            {
                return HF_isEdit.Value == "1";
            }
            set
            {
                HF_isEdit.Value = value ? "1" : "0";
            }
        }
        public void RefreshList()
        {
            LV.Visible = true;
            pnl_new.Visible = false;
            LV.SelectedIndex = -1;
            LDS.DataBind();
            LV.DataBind();
        }

        protected magaUser_DataContext DC_USER;
        protected magaContent_DataContext DC_CONTENT;

        protected void Page_Load(object sender, EventArgs e)
        {
            DC_USER = maga_DataContext.DC_USER;
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack)
            {
                LV.SelectedIndex = -1;
                LDS.DataBind();
                LV.DataBind();
                Bind_drp();
            }
        }
        protected void Bind_drp()
        {
            drp_country.DataBind();
            drp_country.Items.Insert(0, new ListItem("- tutti -", "0"));
            drp_country.Items.Insert(0, new ListItem("- seleziona -", "-1"));
            List<CONT_TBL_LANG> _langs = maga_DataContext.DC_CONTENT.CONT_TBL_LANGs.Where(x => x.is_active == 1).ToList();
            drp_lang.DataSource = _langs;
            drp_lang.DataTextField = "title";
            drp_lang.DataValueField = "id";
            drp_lang.DataBind();
            drp_lang.Items.Insert(0, new ListItem("- tutti -", "0"));
            drp_lang.Items.Insert(0, new ListItem("- seleziona -", "-1"));
            drp_admin.Items.Clear();
            List<USR_ADMIN> _admins = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && (x.rnt_canHaveRequest == 1 || x.rnt_canHaveReservation == 1) && x.is_deleted == 0).ToList();
            foreach (USR_ADMIN usrAdmin in _admins)
            {
                drp_admin.Items.Add(new ListItem("" + usrAdmin.name + " " + usrAdmin.surname, "" + usrAdmin.id));
            }
            drp_admin.Items.Insert(0, new ListItem("- seleziona -", "-1"));
        }

        private void FillControls()
        {
        }

        protected void FillDataFromControls()
        {

            DC_USER.SubmitChanges();
        }

        protected void DisableControls()
        {

            lnk_salva.Visible = false;
            lnk_annulla.Visible = false;
        }
        protected void EnableControls()
        {

            lnk_salva.Visible = true;
            lnk_annulla.Visible = true;
        }
        protected void lnk_salva_Click(object sender, EventArgs e)
        {
            if (drp_country.getSelectedValueInt(0) == -1)
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Selezionare una Location";
                return;
            }
            if (drp_lang.getSelectedValueInt(0) == -1)
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Selezionare una Lingua";
                return;
            }
            if (drp_country.getSelectedValueInt(0) == 0 && drp_lang.getSelectedValueInt(0) == 0)
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Selezionare una Location o una Lingua";
                return;
            }
            if (drp_admin.getSelectedValueInt(0) == -1)
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Selezionare un'Account";
                return;
            }
            USR_RL_COUNTRY_LANG block = DC_USER.USR_RL_COUNTRY_LANGs.SingleOrDefault(item => item.pid_admin == drp_admin.getSelectedValueInt(0) && item.pid_country == drp_country.getSelectedValueInt(0) && item.pid_lang == drp_lang.getSelectedValueInt(0));
            if (block != null)
            {
                lbl_error.Visible = true;
                lbl_error.Text = "La Lingua e Account selezionati sono già abbinati alla Location";
                return;
            }
            int _sequence = 1;
            List<USR_RL_COUNTRY_LANG> _allCollection =
                DC_USER.USR_RL_COUNTRY_LANGs.Where(x => x.pid_country == drp_country.getSelectedValueInt(0) && x.pid_lang == drp_lang.getSelectedValueInt(0)).OrderBy(x => x.sequence).ToList();
            if (_allCollection.Count != 0)
            {
                if (_allCollection.Count() == 2)
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "La Location non puo avere più di 2 Account per Lingua";
                    return;
                }
                _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
            }
            USR_RL_COUNTRY_LANG _newTBL = new USR_RL_COUNTRY_LANG();
            _newTBL.pid_country = drp_country.getSelectedValueInt(0).Value;
            _newTBL.pid_admin = drp_admin.getSelectedValueInt(0).Value;
            _newTBL.pid_lang = drp_lang.getSelectedValueInt(0).Value;
            _newTBL.sequence = _sequence;
            DC_USER.USR_RL_COUNTRY_LANGs.InsertOnSubmit(_newTBL);
            DC_USER.SubmitChanges();
            LDS.DataBind();
            LV.DataBind();
            LV.Visible = true;
            pnl_new.Visible = false;
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            LV.Visible = true;
            pnl_new.Visible = false;
        }

        protected void lnk_new_Click(object sender, EventArgs e)
        {
            LV.Visible = false;
            pnl_new.Visible = true;
            lbl_error.Visible = false;
            if (IdCountry != -1)
            {
                drp_country.setSelectedValue("" + IdCountry);
                drp_country.Enabled = false;
            }
            else
            {
                drp_country.setSelectedValue("-1");
                drp_country.Enabled = true;
            }
            if (IdLang != -1)
            {
                drp_lang.setSelectedValue("" + IdLang);
                drp_lang.Enabled = false;
            }
            else
            {
                drp_lang.setSelectedValue("-1");
                drp_lang.Enabled = true;
            }
            if (IdAdmin != -1)
            {
                drp_admin.setSelectedValue("" + IdAdmin);
                drp_admin.Enabled = false;
            }
            else
            {
                drp_admin.setSelectedValue("-1");
                drp_admin.Enabled = true;
            }
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_pid_country = e.Item.FindControl("lbl_pid_country") as Label;
            Label lbl_pid_admin = e.Item.FindControl("lbl_pid_admin") as Label;
            Label lbl_pid_lang= e.Item.FindControl("lbl_pid_lang") as Label;
            if (e.CommandName == "move_up")
            {
                var block = DC_USER.USR_RL_COUNTRY_LANGs.SingleOrDefault(item => item.pid_admin == lbl_pid_admin.Text.ToInt32() && item.pid_country == lbl_pid_country.Text.ToInt32() && item.pid_lang == lbl_pid_lang.Text.ToInt32());
                var upper_block_list = DC_USER.USR_RL_COUNTRY_LANGs.Where(item => item.sequence < block.sequence && item.pid_country == lbl_pid_country.Text.ToInt32() && item.pid_lang == lbl_pid_lang.Text.ToInt32()).OrderByDescending(x => x.sequence);
                if (upper_block_list.Count() != 0)
                {
                    var upper_block = upper_block_list.First();
                    block.sequence = upper_block.sequence;
                    upper_block.sequence = upper_block.sequence + 1;
                    DC_USER.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();
                }
                else if (block.sequence > 1)
                {
                    block.sequence = 1;
                    DC_USER.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();

                }
            }
            if (e.CommandName == "move_down")
            {
                var block = DC_USER.USR_RL_COUNTRY_LANGs.SingleOrDefault(item => item.pid_admin == lbl_pid_admin.Text.ToInt32() && item.pid_country == lbl_pid_country.Text.ToInt32() && item.pid_lang == lbl_pid_lang.Text.ToInt32());
                var down_block_list = DC_USER.USR_RL_COUNTRY_LANGs.Where(item => item.sequence > block.sequence && item.pid_country == lbl_pid_country.Text.ToInt32() && item.pid_lang == lbl_pid_lang.Text.ToInt32()).OrderBy(x => x.sequence);
                if (down_block_list.Count() != 0)
                {
                    var down_block = down_block_list.First();
                    block.sequence = down_block.sequence;
                    down_block.sequence = down_block.sequence - 1;
                    DC_USER.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();
                }
            }
            if (e.CommandName == "elimina")
            {
                var block = DC_USER.USR_RL_COUNTRY_LANGs.SingleOrDefault(item => item.pid_admin == lbl_pid_admin.Text.ToInt32() && item.pid_country == lbl_pid_country.Text.ToInt32() && item.pid_lang == lbl_pid_lang.Text.ToInt32());
                if (block != null)
                {
                    DC_USER.USR_RL_COUNTRY_LANGs.DeleteOnSubmit(block);
                    DC_USER.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();
                }
            }
            if (e.CommandName == "edit_block")
            {
                LV.Visible = false;
                //pnl_edit_movie.Visible = true;
                //UC_block.ShowSubTitle = false;
                //UC_block.ShowImg = true;
                //UC_block.ShowDelay = false;
                //UC_block.ShowSummary = false;
                //UC_block.ShowDesc = true;
                //UC_block.CollectionID = CollectionID;
                //UC_block.BlockID = lbl_id.Text;
            }
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            PlaceHolder PH_edit = e.Item.FindControl("PH_edit") as PlaceHolder;
            if (PH_edit != null)
                PH_edit.Visible = IdAdmin == -1;
        }

        protected void LV_DataBound(object sender, EventArgs e)
        {
            //LinkButton lnk_new = LV.FindControl("lnk_new") as LinkButton;
            //if (lnk_new != null)
            //    lnk_new.Visible = (IdCountry != 0 && isEdit);
        }
        protected string getCountryName(int id)
        {
            if (id == 0)
                return "-tutti-";
            return AdminUtilities.zone_countryName(id);
        }

        protected string getLangName(int id)
        {
            if (id == 0)
                return "-tutti-";
            return contUtils.getLang_title(id);
        }
    }
}