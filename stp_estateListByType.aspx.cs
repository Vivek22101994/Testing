using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome
{
    public partial class stp_estateListByType : mainBasePage
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
                ErrorLog.addLog(_ip, "stp_estateListByType", _params);
                Response.Redirect(CurrentAppSettings.ERROR_PAGE + "?fr=gl");
                return;
            }
            RewritePath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HF_id.Value = PAGE_REF_ID.ToString();
                HF_lang.Value = CurrentLang.ID.ToString();
                HF_currType.Value = Request.QueryString["currType"];
                clTypeFilter _tf = clUtils.getConfig(CURRENT_SESSION_ID).typeFilters.SingleOrDefault(x => x.currType == HF_currType.Value);
                if (_tf == null) _tf = new clTypeFilter(HF_currType.Value);
                HF_searchTitle.Value = _tf.searchTitle;
                HF_orderBy.Value = _tf.orderBy;
                HF_orderHow.Value = _tf.orderHow;
                HF_currPage.Value = _tf.currPage.ToString();
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
                ltr_description.Text = _stp.description;
            }
        }
    }
}
