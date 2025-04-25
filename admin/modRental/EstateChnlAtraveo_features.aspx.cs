using RentalInRome.data;
using ModRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MagaRentalCE.admin.modRental
{
    public partial class EstateChnlAtraveo_features : adminBasePage
    {
        public int IdEstate
        {
            get
            {
                return HF_IdEstate.Value.ToInt32();
            }
            set
            {
                HF_IdEstate.Value = value.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdEstate = Request.QueryString["id"].ToInt32();

                RNT_TB_ESTATE currEstate = maga_DataContext.DC_RENTAL.RNT_TB_ESTATE.SingleOrDefault(x => x.id == IdEstate);
                if (currEstate == null)
                {
                    Response.Redirect("/admin/rnt_estate_list.aspx");
                    return;
                }
                ltr_apartment.Text = currEstate.code;
                ucNav.IdEstate = IdEstate;
                fillList();
            }
        }
        public void fillList()
        {
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {
                var currList = dcChnl.dbRntChnlAtraveoLkFeatureValuesTBLs.OrderBy(x => x.type).ThenBy(x => x.title).ToList();
                Lv.DataSource = currList;
                Lv.DataBind();
                foreach (ListViewDataItem item in Lv.Items)
                {
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var lbl_type = item.FindControl("lbl_type") as Label;
                    var yesno = item.FindControl("yesno") as CheckBox;
                    var numeric = item.FindControl("numeric") as RadNumericTextBox;
                    var currTbl = dcChnl.dbRntChnlAtraveoEstateFeaturesRLs.SingleOrDefault(x => x.code == lbl_id.Text && x.pidEstate == IdEstate);
                    if (currTbl != null)
                    {
                        yesno.Checked = true;
                        numeric.Value = currTbl.value.ToInt32();
                    }
                    numeric.Visible = lbl_type.Text == "numeric";
                    numeric.Enabled = yesno.Checked;
                }
            }
        }
        public void saveData()
        {
            using (DCchnlAtraveo dcChnl = new DCchnlAtraveo())
            {
                foreach (ListViewDataItem item in Lv.Items)
                {
                    var lbl_id = item.FindControl("lbl_id") as Label;
                    var lbl_type = item.FindControl("lbl_type") as Label;
                    var yesno = item.FindControl("yesno") as CheckBox;
                    var numeric = item.FindControl("numeric") as RadNumericTextBox;
                    if (yesno.Checked)
                    {
                        var currTbl = dcChnl.dbRntChnlAtraveoEstateFeaturesRLs.SingleOrDefault(x => x.code == lbl_id.Text && x.pidEstate == IdEstate);
                        if (currTbl == null)
                        {
                            currTbl = new dbRntChnlAtraveoEstateFeaturesRL();
                            currTbl.code = lbl_id.Text;
                            currTbl.pidEstate = IdEstate;
                            dcChnl.Add(currTbl);
                        }
                        currTbl.value = (lbl_type.Text == "numeric" ? numeric.Value.objToInt32() + "" : "1");
                        dcChnl.SaveChanges();
                    }
                    else
                    {
                        var currTbl = dcChnl.dbRntChnlAtraveoEstateFeaturesRLs.SingleOrDefault(x => x.code == lbl_id.Text && x.pidEstate == IdEstate);
                        if (currTbl != null)
                        {
                            dcChnl.Delete(currTbl);
                            dcChnl.SaveChanges();
                        }
                    }
                }
            }
        }
        protected void lnk_annulla_Click(object sender, EventArgs e)
        {
            fillList();
        }
        protected void lnk_saveOnly_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void chkChanged(object sender, EventArgs e)
        {
            foreach (var item in Lv.Items)
            {
                var lbl_id = item.FindControl("lbl_id") as Label;
                var lbl_type = item.FindControl("lbl_type") as Label;
                var yesno = item.FindControl("yesno") as CheckBox;
                var numeric = item.FindControl("numeric") as RadNumericTextBox;
                numeric.Visible = lbl_type.Text == "numeric";
                numeric.Enabled = yesno.Checked;
            }
        }

    }
}