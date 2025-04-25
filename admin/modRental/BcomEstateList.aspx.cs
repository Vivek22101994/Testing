using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using Telerik.Web.UI;

namespace ModRental.admin.modRental
{
    public partial class BcomEstateList : adminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LDS.DataBind();
            LV.SelectedIndex = -1;
            LV.DataBind();
            if (Request.QueryString["view"]!="true")
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Bulk-upload.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                LV.RenderControl(htmlWrite);
                Response.Write(stringWrite.ToString());
                Response.End();
            }
        }
    }

}