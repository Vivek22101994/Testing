using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class EstateChnlHA_media : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "rnt_estate";
        }
        protected magaRental_DataContext DC_RENTAL;
        protected RNT_TB_ESTATE currTBL;
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
                UC_rnt_estate_navlinks1.IdEstate = value;
            }
        }
        public string CurrType
        {
            get
            {
                return HF_type.Value;
            }
            set
            {
                HF_type.Value = value;
            }
        }
        public string CommonType { get { return "original"; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            using (DCmodRental dc = new DCmodRental())
            {
                if (!IsPostBack)
                {
                    IdEstate = Request.QueryString["id"].ToInt32();
                    currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                    if (currTBL == null)
                    {
                        Response.Redirect("/admin/rnt_estate_list.aspx");
                        return;
                    }
                    ltr_apartment.Text = currTBL.code + " / " + "rif. " + currTBL.id;
                    UC_rnt_estate_navlinks1.IdEstate = IdEstate;
                    CurrType = "chnlHA";
                    LV_gallery_DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setSortable", "setSortable();", true);
                }
            }
        }
        protected string getImageClass(string path)
        {
            if (File.Exists(Path.Combine(App.SRP, path)))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(Path.Combine(App.SRP, path));
                if (img.Width > img.Height) return img.Width < 640 || img.Height < 480 ? "smallimage" : "";
                if (img.Width < img.Height) return img.Width < 480 || img.Height < 640 ? "smallimage" : "";
            }
            return "smallimage";
        }
        protected void pnlDett_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "sortable") saveSequence();
            LV_gallery_DataBind();
        }
        protected void LV_gallery_DataBind()
        {
            var currList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == "homeaway").OrderBy(x => x.sequence).ToList();
            if (currList.Count>0)
            {
                foreach (var tmp in currList)
                    tmp.type = CurrType;
                DC_RENTAL.SubmitChanges();
            }
            currList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CurrType + "").OrderBy(x => x.sequence).ToList();
            var currIds = currList.Select(x => x.id).ToList();
            var currPaths = currList.Select(x => x.img_banner).ToList();
            currList.AddRange(DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CommonType && !currPaths.Contains(x.img_banner)).OrderBy(x => x.sequence).ToList());
            LV_gallery.DataSource = currList;
            LV_gallery.DataBind();
            foreach (ListViewDataItem item in LV_gallery.Items)
            {
                var lbl_id = item.FindControl("lbl_id") as Label;
                var lnkAdd = item.FindControl("lnkAdd") as LinkButton;
                var lnkDel = item.FindControl("lnkDel") as LinkButton;
                var lnkEdit = item.FindControl("lnkEdit") as LinkButton;
                lnkDel.Visible = currIds.Contains(lbl_id.Text.ToInt32());
                lnkAdd.Visible = !lnkDel.Visible;
                lnkEdit.OnClientClick = "OpenShadowbox('EstateMediaDett.aspx?id=" + lbl_id.Text + "'); return false;";
            }
        }
        protected void saveSequence()
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            List<string> _list = HF_order.Value.splitStringToList("|");
            for (int i = 0; i < _list.Count; i++)
            {
                RNT_RL_ESTATE_MEDIA currImg = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == _list[i].ToInt32());
                if (currImg != null)
                {
                    currImg.sequence = i + 1;
                }
            }
            DC_RENTAL.SubmitChanges();
            AppSettings._refreshCache_RNT_ESTATEs();
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "radAlert", "radalert(\"Ordinamento salvato correttamente.\");", true);
        }
        protected void LV_gallery_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            currTBL = DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
            if (currTBL == null)
            {
                Response.Redirect("/admin/rnt_estate_list.aspx");
                return;
            }
            if (e.CommandName == "additem")
            {
                var lbl_id = e.Item.FindControl("lbl_id") as Label;
                var tmp = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (tmp != null)
                {
                    int _sequence = 1;
                    List<RNT_RL_ESTATE_MEDIA> _allCollection =
                        DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CurrType + "").OrderBy(x => x.sequence).ToList();
                    if (_allCollection.Count != 0)
                    {
                        _sequence = _allCollection.Max(x => x.sequence.Value) + 1;
                    }
                    RNT_RL_ESTATE_MEDIA currImg = new RNT_RL_ESTATE_MEDIA();
                    currImg.sequence = _sequence;
                    currImg.pid_estate = IdEstate;
                    currImg.code = "";
                    currImg.type = CurrType + "";
                    currImg.img_banner = tmp.img_banner;
                    currImg.img_thumb = tmp.img_thumb;
                    DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(currImg);
                    DC_RENTAL.SubmitChanges();
                    if (CurrType + "" == "gallery" && tmp.sequence == 1)
                    {
                        currTBL.img_banner = currImg.img_banner;
                        currTBL.img_preview_1 = currImg.img_thumb;
                        DC_RENTAL.SubmitChanges();
                    }
                }
                LV_gallery_DataBind();
            }
            if (e.CommandName == "deleteitem")
            {
                var lbl_id = e.Item.FindControl("lbl_id") as Label;
                var tmp = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.SingleOrDefault(x => x.id == lbl_id.Text.ToInt32());
                if (tmp != null)
                {
                    DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteOnSubmit(tmp);
                    DC_RENTAL.SubmitChanges();
                }
                LV_gallery_DataBind();
            }
        }
        protected void lnkSelectAll_Click(object sender, EventArgs e)
        {
            var currList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CurrType + "").OrderBy(x => x.sequence).ToList();
            var currIds = currList.Select(x => x.id).ToList();
            var currPaths = currList.Select(x => x.img_banner).ToList();
            currList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CommonType && !currPaths.Contains(x.img_banner)).OrderBy(x => x.sequence).ToList();
            foreach (var tmp in currList)
            {
                RNT_RL_ESTATE_MEDIA currImg = new RNT_RL_ESTATE_MEDIA();
                currImg.sequence = tmp.sequence;
                currImg.pid_estate = IdEstate;
                currImg.code = "";
                currImg.type = CurrType + "";
                currImg.img_banner = tmp.img_banner;
                currImg.img_thumb = tmp.img_thumb;
                DC_RENTAL.RNT_RL_ESTATE_MEDIAs.InsertOnSubmit(currImg);
                DC_RENTAL.SubmitChanges();
            }
            LV_gallery_DataBind();
        }
        protected void lnkRemoveAll_Click(object sender, EventArgs e)
        {
            var tmpList = DC_RENTAL.RNT_RL_ESTATE_MEDIAs.Where(x => x.pid_estate == IdEstate && x.type == CurrType).ToList();
            DC_RENTAL.RNT_RL_ESTATE_MEDIAs.DeleteAllOnSubmit(tmpList);
            DC_RENTAL.SubmitChanges();
            LV_gallery_DataBind();
        }
    }
}