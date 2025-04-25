using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin.uc
{
    public partial class UC_cont_collection_blocks : System.Web.UI.UserControl
    {
        private magaContent_DataContext DC_CONTENT;
        public string CollectionID
        {
            get
            {
                return HF_id.Value;
            }
            set
            {
                HF_id.Value = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            lnk_close.OnClientClick = UC_block.Unique + "_removeTinyEditor()";
        }
        protected void LV_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Label lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (e.CommandName == "move_up")
            {
                var block = DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.SingleOrDefault(item => item.pid_block == lbl_id.Text.ToInt32() && item.pid_collection == CollectionID.ToInt32());
                var upper_block_list = DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.Where(item => item.sequence < block.sequence && item.pid_collection == CollectionID.ToInt32()).OrderByDescending(x => x.sequence);
                if (upper_block_list.Count() != 0)
                {
                    var upper_block = upper_block_list.First();
                    block.sequence = upper_block.sequence;
                    upper_block.sequence = upper_block.sequence + 1;
                    DC_CONTENT.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();
                }
                else if (block.sequence > 1)
                {
                    block.sequence = 1;
                    DC_CONTENT.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();

                }
            }
            if (e.CommandName == "move_down")
            {
                var block = DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.SingleOrDefault(item => item.pid_block == lbl_id.Text.ToInt32() && item.pid_collection == CollectionID.ToInt32());
                var down_block_list = DC_CONTENT.CONT_RL_COLLECTION_BLOCKs.Where(item => item.sequence > block.sequence && item.pid_collection == CollectionID.ToInt32()).OrderBy(x => x.sequence);
                if (down_block_list.Count() != 0)
                {
                    var down_block = down_block_list.First();
                    block.sequence = down_block.sequence;
                    down_block.sequence = down_block.sequence - 1;
                    DC_CONTENT.SubmitChanges();
                    LDS.DataBind();
                    LV.DataBind();
                }
            }
            if (e.CommandName == "edit_block")
            {
                LV.Visible = false;
                pnl_edit_movie.Visible = true;
                UC_block.ShowSubTitle = false;
                UC_block.ShowImg = true;
                UC_block.ShowDelay = false;
                UC_block.ShowSummary = false;
                UC_block.ShowDesc = true;
                UC_block.CollectionID = CollectionID;
                UC_block.BlockID = lbl_id.Text;
            }
        }
        protected void lnk_new_Click(object sender, EventArgs e)
        {
            LV.Visible = false;
            pnl_edit_movie.Visible = true;
            UC_block.ShowSubTitle = false;
            UC_block.ShowImg = true;
            UC_block.ShowDelay = false;
            UC_block.ShowSummary = false;
            UC_block.ShowDesc = true;
            UC_block.CollectionID = CollectionID;
            UC_block.BlockID = "0";
        }
        protected void lnk_close_Click(object sender, EventArgs e)
        {
            pnl_edit_movie.Visible = false;
            LV.Visible = true;
            LDS.DataBind();
            LV.DataBind();
        }
    }
}