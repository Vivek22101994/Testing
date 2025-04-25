using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RentalInRome.ucMain
{
    public partial class ucHeader : System.Web.UI.UserControl
    {
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = CommonUtilities.newUniqueID();
                return HF_unique.Value;
            }
        }

        public string CURRENT_SESSION_ID
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                return m.CURRENT_SESSION_ID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                check_agentAuth();
            }
        }

        #region
        protected void check_agentAuth()
        {
            if (affiliatesarea.agentAuth.CurrentID == 0)
            {
                pnl_agentAuthOK.Visible = false;
                pnl_agentAuthNO.Visible = true;
            }
            else
            {
                pnl_agentAuthOK.Visible = true;
                pnl_agentAuthNO.Visible = false;
                ltr_agentAuth_nameFull.Text = affiliatesarea.agentAuth.CurrentName;
            }
        }
        #endregion

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            HyperLink _hl = e.Item.FindControl("HL") as HyperLink;
            Label _lbl_id = e.Item.FindControl("lbl_id") as Label;
            if (_hl != null)
            {
                _hl.NavigateUrl = null;
                _hl.Enabled = true;
                if (_lbl_id == null)
                    return;
                if (_lbl_id.Text == CurrentLang.ID.ToString())
                {
                    //_hl.CssClass = "active";
                    //_hl.Enabled = false;
                    HtmlGenericControl liLang = e.Item.FindControl("liLang") as HtmlGenericControl;
                    if (liLang != null) liLang.Visible = false;
                    Literal ltrCurrLang = LV.FindControl("ltrCurrLang") as Literal;
                    if (ltrCurrLang != null) ltrCurrLang.Text = _hl.Text;
                    return;
                }
                string _path = GetCurrentLangPath(_lbl_id.Text);
                if (_path != "")
                {
                    _hl.NavigateUrl = _path;
                    _hl.Enabled = true;
                }
                else
                {
                    _hl.NavigateUrl = null;
                    _hl.Enabled = false;
                    _hl.CssClass = "disable";
                }
            }
        }

        protected string GetCurrentLangPath(string id_lang)
        {
            mainBasePage m = (mainBasePage)this.Page;
            string pt = m.PAGE_TYPE;
            int pid = m.PAGE_REF_ID;
            if (pt == "pg_estate" && Request.QueryString["dtS"].objToInt32() != 0 && Request.QueryString["dtE"].objToInt32() != 0 && Request.QueryString["numPers"].objToInt32() != 0)
                return CurrentSource.getPagePath(pid.ToString(), pt, id_lang) + "?dtS=" + Request.QueryString["dtS"] + "&dtE=" + Request.QueryString["dtE"] + "&numPers=" + Request.QueryString["numPers"];
            else
                return CurrentSource.getPagePath(pid.ToString(), pt, id_lang);
        }
        protected string getLinkClass(string id, string type)
        {
            mainBasePage mbp = this.Page as mainBasePage;
            if (mbp != null)
            {
                return mbp.PAGE_REF_ID.ToString() == id && mbp.PAGE_TYPE == type ? "active" : "";
            }
            return CommonUtilities.newUniqueID();
        }
    }
}