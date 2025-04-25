using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class mail_iframe_message_body : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "usr_mail";
        }
        protected magaMail_DataContext DC_MAIL;
        private MAIL_TBL_MESSAGE _currTBL;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_MAIL = maga_DataContext.DC_MAIL;
            if (!IsPostBack)
            {
                Response.Clear();
                int _id;
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out _id))
                {
                    _currTBL = DC_MAIL.MAIL_TBL_MESSAGE.SingleOrDefault(x => x.id == _id);
                    if (_currTBL == null)
                        return;
                    Response.Write((_currTBL.body_html_text != "") ? _currTBL.body_html_text : _currTBL.body_plain_text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));

                }
            }
        }
    }
}
