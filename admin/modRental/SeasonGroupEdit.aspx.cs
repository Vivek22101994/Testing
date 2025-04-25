using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class SeasonGroupEdit : adminBasePage
    {
        protected RntSeasonGroupTBL currTBL;
        protected int CurrId
        {
            get { return HfId.Value.ToInt32(); }
            set { HfId.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrId = Request.QueryString["id"].ToInt32();
                if (CurrId == 0)
                {
                    CloseRadWindow("reload");
                    return;
                }
                HF_callbackFunction.Value = Request.QueryString["callback"];
                fillData();
            }
        }
        
        private void fillData()
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                currTBL = dc.RntSeasonGroupTBL.SingleOrDefault(x => x.id == CurrId);
                if (currTBL == null)
                {
                    currTBL = new RntSeasonGroupTBL();
                    drp_pidSeasonGroup.DataSource = dc.RntSeasonGroupTBL.Where(x => x.id != 0).OrderBy(x => x.code).ToList();
                    drp_pidSeasonGroup.DataTextField = "code";
                    drp_pidSeasonGroup.DataValueField = "id";
                    drp_pidSeasonGroup.DataBind();
                    drp_pidSeasonGroup.Items.Insert(0, new ListItem("-stagionalità predefinita-", "0"));
                    ltrTitle.Text = "Creazione della nuova stagionalità";
                }
                else
                {
                    drp_pidSeasonGroup.DataSource = dc.RntSeasonGroupTBL.Where(x => x.id != CurrId && x.id != 0).OrderBy(x => x.code).ToList();
                    drp_pidSeasonGroup.DataTextField = "code";
                    drp_pidSeasonGroup.DataValueField = "id";
                    drp_pidSeasonGroup.DataBind();
                    drp_pidSeasonGroup.Items.Insert(0, new ListItem("-stagionalità predefinita-", "0"));
                    drp_pidSeasonGroup.Items.Insert(0, new ListItem("- non toccare le date -", "-1"));
                    ltrTitle.Text = "Modifica della stagionalità: " + currTBL.code;
                }
                txt_code.Text = currTBL.code;
                re_description.Content = currTBL.description;
                
            }
        }
        private void saveData()
        {
            using (magaRental_DataContext dc = maga_DataContext.DC_RENTAL)
            {
                currTBL = dc.RntSeasonGroupTBL.SingleOrDefault(x => x.id == CurrId);
                if (currTBL == null)
                {
                    currTBL = new RntSeasonGroupTBL();
                    dc.RntSeasonGroupTBL.InsertOnSubmit(currTBL);
                }
                currTBL.code = txt_code.Text;
                currTBL.description = re_description.Content;
                dc.SubmitChanges();
                CurrId = currTBL.id;
                if (drp_pidSeasonGroup.getSelectedValueInt() != -1)
                {
                    dc.RntSeasonDatesTBL.DeleteAllOnSubmit(dc.RntSeasonDatesTBL.Where(x => x.pidSeasonGroup == CurrId));
                    dc.SubmitChanges();
                    var tmpList = dc.RntSeasonDatesTBL.Where(x => x.pidSeasonGroup == drp_pidSeasonGroup.getSelectedValueInt() && x.dtEnd >= DateTime.Now).ToList();
                    foreach (var tmp in tmpList)
                    {
                        var tmpNew = new RntSeasonDatesTBL();
                        tmpNew.pidSeasonGroup = CurrId;
                        tmpNew.pidPeriod = tmp.pidPeriod;
                        tmpNew.dtStart = tmp.dtStart;
                        tmpNew.dtEnd = tmp.dtEnd;
                        dc.RntSeasonDatesTBL.InsertOnSubmit(tmpNew);
                    }
                    dc.SubmitChanges();
                }
                if (HF_callbackFunction.Value != "")
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "parent." + HF_callbackFunction.Value + "(" + CurrId + ");", true);
                else
                    CloseRadWindow("reload");
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }

    }
}

