using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;

namespace RentalInRome.admin
{
    public partial class manage_static_page_multimedia : adminBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PAGE_TYPE = "superadmin";
        }
        private magaContent_DataContext DC_CONTENT;
        protected void Page_Load(object sender, EventArgs e)
        {
            DC_CONTENT = maga_DataContext.DC_CONTENT;
            if (!IsPostBack)
            {
                CONT_TBL_PAGE _item = DC_CONTENT.CONT_TBL_PAGEs.SingleOrDefault(x => x.id == int.Parse(Request.QueryString["id"]));
                if (_item == null)
                    Response.Redirect("/admin/");
                UC_static_page_multimedia1.StaticPageID = _item.id.ToString();

            }
        }
    }
}
