using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ModRental.admin.modRental
{
    public partial class EstateLogDett : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (DCmodRental dc = new DCmodRental())
                {
                    Guid uid = new Guid(Request.QueryString["id"]);
                    var currTBL = dc.dbRntEstateLOGs.SingleOrDefault(x => x.uid == uid);
                    if (currTBL == null) return;
                    ltr_Date.Text = "" + currTBL.logDate;
                    ltr_estateCode.Text = currTBL.estateCode;
                    ltr_userName.Text = currTBL.userName;
                    ltr_changeField.Text = currTBL.changeField;
                    txt_valueBefore.Text = currTBL.valueBefore;
                    txt_valueAfter.Text = currTBL.valueAfter;
                    RadAjaxPanel1.Visible = true;
                }
            }
        }
    }
}