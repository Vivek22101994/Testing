using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class stp_privacy : mainBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.PAGE_TYPE = "stp";
            int id = Request.QueryString["id"].ToInt32();
            if (id != 0)
                base.PAGE_REF_ID = id;
            else
            {
                string _params = "";
                string _ip = "";
                try { _ip = Request.ServerVariables.Get("REMOTE_HOST"); }
                catch (Exception ex1) { }
                foreach (string key in Request.Params.AllKeys)
                    _params += "\n" + key + "=" + Request.Params[key];
                ErrorLog.addLog(_ip, "stp_privacy", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
            }
            RewritePath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Fill_data();
            }
        }
        protected void Fill_data()
        {
            CONT_VIEW_STP _stp =
                maga_DataContext.DC_CONTENT.CONT_VIEW_STPs.SingleOrDefault(item => item.id == PAGE_REF_ID && item.pid_lang == CurrentLang.ID);
            if (_stp != null)
            {
                ltr_meta_description.Text = _stp.meta_description;
                ltr_meta_keywords.Text = _stp.meta_keywords;
                ltr_meta_title.Text = _stp.meta_title;
                ltr_title.Text = _stp.title;
                ltr_sub_title.Text = _stp.sub_title;
                ltr_description.Text = _stp.description;
            }
        }
    }
}

