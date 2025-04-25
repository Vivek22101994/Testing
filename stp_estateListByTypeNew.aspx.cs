using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome
{
    public partial class stp_estateListByTypeNew : contStpBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // HF_id.Value = PAGE_REF_ID.ToString();
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

            if (currStp != null)
            {
                ltr_meta_description.Text = currStp.meta_description;
                ltr_meta_keywords.Text = currStp.meta_keywords;
                ltr_meta_title.Text = currStp.meta_title;
                ltr_title.Text = currStp.title;
                ltr_description.Text = currStp.description;
            }
        }
    }
}