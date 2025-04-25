using RentalInRome.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.ucMain
{
    public partial class ucBreadcrumbs : System.Web.UI.UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!IsPostBack)
            {
                mainBasePage _p = this.Page as mainBasePage;
                List<BC_item> _list = _p.BC_list;

                if (_list != null && _list.Count > 0)
                {
                    lbl_current.Text = _list[0].Title;
                    if (_list.Count > 1)
                    {
                        HL_3.NavigateUrl = _list[1].Url;
                        HL_3.Text = _list[1].Title;
                        li_3.Visible = true;
                    }
                    if (_list.Count > 2)
                    {
                        HL_2.NavigateUrl = _list[2].Url;
                        HL_2.Text = _list[2].Title;
                        li_2.Visible = true;
                    }
                    if (_list.Count > 3)
                    {
                        HL_1.NavigateUrl = _list[3].Url;
                        HL_1.Text = _list[3].Title;
                        li_1.Visible = true;
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}