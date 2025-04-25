using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModAuth;
using Telerik.Web.UI;

namespace RentalInRome.reservationarea
{
    public partial class agentclientdata : basePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (resUtils.CurrentIdReservation_gl != 0 && resUtils.CurrentIdReservation_gl < 150000)
            {
                Response.Redirect("/reservationarea/arrivaldeparture.aspx", true);
                return;
            }
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }
        public long IdAgent
        {
            get
            {
                return HF_idAgent.Value.ToInt64();
            }
            set
            {
                HF_idAgent.Value = value.ToString();
            }
        }
        public long IdClient
        {
            get
            {
                return HF_id.Value.ToInt64();
            }
            set
            {
                HF_id.Value = value.ToString();
            }
        }
        private dbAuthClientTBL currClient;
        public dbAuthClientTBL tblClient
        {
            get
            {
                if (currClient == null)
                    using (DCmodAuth dc = new DCmodAuth())
                        currClient = dc.dbAuthClientTBLs.SingleOrDefault(x => x.id == IdClient);
                return currClient ?? new dbAuthClientTBL();
            }
        }
        private magaRental_DataContext DC_RENTAL;
        private RNT_TBL_RESERVATION _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                fillData();
            }
       }
        protected void pnlFascia_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            fillData();
        }
        protected void drp_client_DataBind()
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                drp_client.DataSource = dc.dbAuthClientTBLs.Where(x => x.pidAgent == IdAgent && x.isActive == 1).OrderBy(x => x.nameFull);
                drp_client.DataTextField = "nameFull";
                drp_client.DataValueField = "id";
                drp_client.DataBind();
                drp_client.Items.Insert(0, new ListItem("- - -", "0"));
            }
        }
        protected void fillData()
        {
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            if (_currTBL.agentID == 0)
            {
                Response.Redirect("/reservationarea/", true);
                return;
            }
            IdAgent = _currTBL.agentID.objToInt64();
            drp_client_DataBind();
            RNT_TB_ESTATE _estTB = AppSettings.RNT_TB_ESTATE.SingleOrDefault(x => x.id == _currTBL.pid_estate);
            if (_estTB == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }

            if (_currTBL.agentClientID.objToInt64() == 0)
            {
                pnl_view.Visible = false;
                pnl_edit.Visible = true;
            }
            else
            {
                pnl_view.Visible = true;
                pnl_edit.Visible = false;
                IdClient = _currTBL.agentClientID.objToInt64();
            }

            #region Hide "Chosse Another Client" link
            string agentIDs = CommonUtilities.getSYS_SETTING("resarea_restrictagentIds");
            List<long> restrictedAgentIds = agentIDs.splitStringToList(",").Select(x => x.objToInt64()).ToList();
            if (restrictedAgentIds.Contains(IdAgent))
            {
                LinkButton1.Visible = false;
            }
            #endregion
        }
        protected void saveData()
        {
            var tmp = new dbAuthClientTBL();
            using (DCmodAuth dc = new DCmodAuth())
            {
                tmp = dc.dbAuthClientTBLs.SingleOrDefault(x => x.id == drp_client.SelectedValue.ToInt64());
                if (tmp == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"Please select a client. \");", true);
                    return;
                }
            }
            _currTBL = DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == CurrentIdReservation);
            if (_currTBL == null)
            {
                Response.Redirect("/reservationarea/login.aspx", true);
                return;
            }
            _currTBL.agentClientID = tmp.id;
            DC_RENTAL.SubmitChanges();
            pnl_view.Visible = true;
            pnl_edit.Visible = false;
            IdClient = _currTBL.agentClientID.objToInt64();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "callAlert", "callAlert(\"Data was successfully saved. \");", true);
        }
        protected void lnk_save_Click(object sender, EventArgs e)
        {
            saveData();
        }
        protected void lnk_change_Click(object sender, EventArgs e)
        {
            pnl_view.Visible = false;
            pnl_edit.Visible = true;
        }
        protected void lnk_cancel_Click(object sender, EventArgs e)
        {
            fillData();
        }
    }
}