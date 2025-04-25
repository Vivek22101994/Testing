using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.common
{
    public partial class MP_areares : System.Web.UI.MasterPage
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
        public string currPageType
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                if (m == null)
                    return "";
                return m.PAGE_TYPE;
            }
        }
        public int currPageID
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                if (m == null)
                    return 0;
                return m.PAGE_REF_ID;
            }
        }
        public string currPagePath
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                if (m == null)
                    return "";
                string pt = m.PAGE_TYPE;
                int pid = m.PAGE_REF_ID;
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + CurrentSource.getPagePath(pid.ToString(), pt, CurrentLang.ID.ToString());
            }
        }
        public string currPageName
        {
            get
            {
                mainBasePage m = (mainBasePage)this.Page;
                if (m == null)
                    return "";
                string pt = m.PAGE_TYPE;
                int pid = m.PAGE_REF_ID;
                return CurrentSource.getPageName(pid.ToString(), pt, CurrentLang.ID.ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setToolTip", "setToolTip();", true);
        }

        protected void lnk_getPdf_Click(object sender, EventArgs e)
        {
            CommonUtilities.downloadPdfFromUrl(currPagePath, currPageName != "" ? currPageName.clearPathName() + ".pdf" : "RomeApartment.pdf", 0.3f, 0.3f, 0.3f, 0.3f);
        }
    }
}
