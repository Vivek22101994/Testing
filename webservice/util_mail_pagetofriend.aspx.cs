//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace RentalInRome.webservice
//{
//    public partial class util_mail_pagetofriend : System.Web.UI.Page
//    {
//        private int _currLang;
//        private string _senderName;
//        private string _receiverMail;
//        private string _currPagePath;
//        private string _currPageName;
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                Response.Clear();
//                Response.Buffer = true;
//                Response.ClearContent();
//                Response.ClearHeaders();
//                Response.ContentType = "text/html";
//                _senderName = Request.QueryString["senderName"].urlDecode();
//                _receiverMail = Request.QueryString["receiverMail"].urlDecode();
//                _currPagePath = Request.QueryString["currPagePath"].urlDecode();
//                _currPageName = Request.QueryString["currPageName"].urlDecode();
//                if (_currPagePath.Trim() == "") return;
//                if (_currPageName.Trim() == "") _currPageName = _currPagePath;
//                _currLang = Request.QueryString["lang"].objToInt32();
//                if (_currLang == 0)
//                    _currLang = CurrentLang.ID;
//                string _subject = MailingUtilities.mailTemplate_subject("cl_mail_pagetofriend", _currLang, "Error");
//                if (_subject == "Error" && _currLang != 2)
//                {
//                    _currLang = 2;
//                    _subject = MailingUtilities.mailTemplate_subject("cl_mail_pagetofriend", _currLang, "Error");
//                }
//                if (_subject == "Error")
//                {
//                    _currLang = 1;
//                    _subject = MailingUtilities.mailTemplate_subject("cl_mail_pagetofriend", _currLang, "Error");
//                }
//                string _body = MailingUtilities.mailTemplate_body("cl_mail_pagetofriend", _currLang, "Error");
//                string _link = "<a href=\"" + _currPagePath + "\">" + _currPageName + "</a>";
//                _body = _body.Replace("#sender_name#", _senderName);
//                _body = _body.Replace("#page_link#", _link);
//                bool _bcc = CommonUtilities.getSYS_SETTING("util_mail_pagetofriend_bcc") != "false";
//                string _response = MailingUtilities.autoSendMailTo(_subject, _body, _receiverMail, _bcc, "util_mail_pagetofriend") ? "ok" : "ko";
//                Response.Write(_response);
//            }
//        }
//    }
//}
