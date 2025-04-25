using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.webservice
{
    public partial class authClientLoginXml : System.Web.UI.Page
    {
        private magaRental_DataContext DC_RENTAL;
        private string CURRENT_SESSION_ID;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_RENTAL = maga_DataContext.DC_RENTAL;
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                CURRENT_SESSION_ID = Request["SESSION_ID"];
                string action = "" + Request["action"];
                string returnString = "";
                if (action == "login")
                {
                    string client_login = "" + Request["client_login"];
                    string client_pwd = "" + Request["client_pwd"];
                    if (client_login.Trim() != "" && client_pwd.Trim() != "")
                    {
                        var _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.FirstOrDefault(x => x.is_deleted != 1 && x.login == client_login && x.password == client_pwd);
                        if (_client == null)
                        {
                            Response.Write("<error>WrongUsernameOrPassword</error>");
                            Response.End();
                            return;
                        }
                        if (_client.is_active != 1)
                        {
                            Response.Write("<error>Disabled</error>");
                            Response.End();
                            return;
                        }
                        returnString += "<client>";
                        returnString += "<id>" + _client.id + "</id>";
                        returnString += "<name_full>" + _client.name_full + "</name_full>";
                        returnString += "<email>" + _client.contact_email + "</email>";
                        returnString += "<country>" + _client.loc_country + "</country>";
                        returnString += "<contact_phone>" + _client.contact_phone + "</contact_phone>";
                        returnString += "</client>";
                        Response.Write(returnString);
                        Response.End();
                        return;
                    }
                    Response.Write("<error>WrongUsernameOrPassword</error>");
                    Response.End();
                    return;
                }
                else
                {
                    string recovery_login = "" + Request["recovery_login"];
                    if (recovery_login.Trim() != "")
                    {
                        var _client = maga_DataContext.DC_USER.USR_TBL_CLIENTs.FirstOrDefault(x => x.is_deleted != 1 && x.contact_email == recovery_login);
                        if (_client == null)
                        {
                            Response.Write("<error>NotExist</error>");
                            Response.End();
                            return;
                        }
                        if (_client.is_active != 1)
                        {
                            Response.Write("<error>Disabled</error>");
                            Response.End();
                            return;
                        }
                        if (AdminUtilities.usrClient_mailPwdRecovery(_client.id))
                            Response.Write("ok");
                        else
                            Response.Write("<error>MailError</error>");
                        Response.End();
                        return;
                    }
                    Response.Write("<error>NotExist</error>");
                    Response.End();
                    return;
                }
            }
        }
    }
}
