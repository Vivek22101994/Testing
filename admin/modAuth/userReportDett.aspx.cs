using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModAuth.admin.modAuth
{
    public partial class userReportDett : adminBasePage
    {
        protected dbAuthUserReportTBL currTBL;
        protected long IdReport
        {
            get { return HfId.Value.ToInt64(); }
            set { HfId.Value = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool canView = true;
                IdReport = Request.QueryString["id"].ToInt64();
                drp_pidUser_DataBind();
                if (UserAuthentication.CURRENT_USER_ROLE != 1)
                {
                    drp_pidUser.Enabled = false;
                    List<int> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.hasAuthUserReport == 1).Select(x => x.id).ToList();
                    if (!_list.Contains(UserAuthentication.CurrentUserID))
                    {
                        canView = false;
                    }
                    else
                    {
                        using (DCmodAuth dc = new DCmodAuth())
                        {
                            currTBL = dc.dbAuthUserReportTBLs.SingleOrDefault(x => x.id == IdReport);
                            if (currTBL != null && currTBL.pidUser != UserAuthentication.CurrentUserID)
                            {
                                canView = false;
                            }
                        }
                    }
                }
                if (!canView)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "alert(\"Non hai permessi per questa pagina. Contattare l'amministratore.\");CloseRadWindow('reload');", true);
                    pnlDett.Visible = false;
                    return;
                }
                drp_repType_DataBind();
                fillData();
            }
        }
        protected void drp_pidUser_DataBind()
        {
            drp_pidUser.Items.Clear();
            List<USR_ADMIN> _list = maga_DataContext.DC_USER.USR_ADMIN.Where(x => x.is_active == 1 && x.is_deleted != 1 && x.hasAuthUserReport == 1).OrderBy(x => x.name).ToList();
            foreach (USR_ADMIN _admin in _list)
            {
                drp_pidUser.Items.Add(new ListItem("" + _admin.name + " " + _admin.surname, "" + _admin.id));
            }
            drp_pidUser.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        protected void drp_repType_DataBind()
        {
            drp_repType.Items.Clear();
            drp_repType.DataSource = authProps.UserReportType;
            drp_repType.DataTextField = "title";
            drp_repType.DataValueField = "code";
            drp_repType.DataBind();
            drp_repType.Items.Insert(0, new ListItem("- tutti -", ""));
        }
        private void fillData()
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                currTBL = dc.dbAuthUserReportTBLs.SingleOrDefault(x => x.id == IdReport);
                if (currTBL == null)
                {
                    IdReport = 0;
                    currTBL = new dbAuthUserReportTBL();
                    ltrTitle.Text = "Nuovo Record in Report";
                    currTBL.repDateTime = DateTime.Now;
                }
                else
                    ltrTitle.Text = "Scheda Report #:" + currTBL.code;

                drp_pidUser.setSelectedValue(currTBL.pidUser.ToString());
                drp_repType.setSelectedValue(currTBL.repType);
                txt_repTitle.Text = currTBL.repTitle;
                rdtp_repDateTime.SelectedDate = currTBL.repDateTime;
                re_repDesc.Content = currTBL.repDesc;
            }
        }
        private void saveData()
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                currTBL = dc.dbAuthUserReportTBLs.SingleOrDefault(x => x.id == IdReport);
                if (currTBL == null)
                {
                    currTBL = new dbAuthUserReportTBL();
                    currTBL.uid = Guid.NewGuid();
                    currTBL.createdDate = DateTime.Now;
                    currTBL.createdUserID = UserAuthentication.CurrentUserID;
                    currTBL.createdUserNameFull = UserAuthentication.CurrentUserName;
                    dc.Add(currTBL);
                    dc.SaveChanges();
                    currTBL.code = currTBL.id.ToString().fillString("0", 6, false);
                    IdReport = currTBL.id;
                    currTBL.pidUser = (UserAuthentication.CURRENT_USER_ROLE == 1) ? drp_pidUser.getSelectedValueInt(0) : UserAuthentication.CurrentUserID;
                }
                else
                    currTBL.pidUser = drp_pidUser.getSelectedValueInt(0);

                currTBL.repType = drp_repType.SelectedValue;
                currTBL.repTitle = txt_repTitle.Text;
                currTBL.repDateTime = rdtp_repDateTime.SelectedDate;
                currTBL.repDesc = re_repDesc.Content;

                dc.SaveChanges();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "errorAlert", "radalert(\"Le modifiche sono state salvate con successo.\", 340, 110);", true);
                fillData();
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            saveData();
            //CloseRadWindow("reload");
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            fillData();
        }

    }
}

