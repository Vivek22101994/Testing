using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModRental;

namespace RentalInRome.reservationarea
{
    public partial class UC_breadcrumb : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Visible = setHLs();
            }
        }
        public int langId
        {
            get
            {
                return hfd_lang.Value.objToInt32();
            }
            set
            {
                hfd_lang.Value = value.ToString();
            }
        }
        protected bool setHLs()
        {
            basePage m = (basePage)this.Page;
            if (m == null)
                return false;
            string pt = m.PAGE_TYPE;
            int pid = m.PAGE_REF_ID;
            langId = App.LangID;
            //if (App.LangID == 1)
            //{
            //    langId = 1;
            //}
            //else
            //{
            //    langId = 2;
            //}
            if (pt == "extra_category")
            {
                if (Session["category"] != null)
                {
                    List<int> lstCategory = (List<int>)Session["category"];

                    //if (lstCategory.Count == 1)
                    //{
                        
                    //    HL_1.Visible = true;
                    //    HL_1.Text = get_categoryName(lstCategory[0]);
                    //    HL_1.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[0];
                    //}
                    if (lstCategory.Count == 2)
                    {
                        HL_1.Visible = true;
                        HL_1.Text = get_categoryName(lstCategory[0]);
                        HL_1.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[1];

                        //HL_2.Visible = true;
                        //HL_2.Text = get_categoryName(lstCategory[1]);
                        //HL_2.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[1];
                    }
                    else if (lstCategory.Count > 2)
                    {
                        HL_1.Visible = true;
                        HL_1.Text = get_categoryName(lstCategory[lstCategory.Count-3]);
                        HL_1.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[lstCategory.Count - 3];

                        HL_2.Visible = true;
                        HL_2.Text = get_categoryName(lstCategory[lstCategory.Count-2]);
                        HL_2.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[lstCategory.Count - 2];
                    }
                }

                lbl_current.Text = get_categoryName(pid);

            }
            if (pt == "extra_booking")
            {
                //if (Session["category"] != null)
                //{
                //    HL_1.Visible = true;
                //    HL_1.Text = get_categoryName(Session["category"].objToInt32());
                //    HL_1.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + Session["category"].objToInt32();

                //}
                if (Session["category"] != null)
                {
                    List<int> lstCategory = (List<int>)Session["category"];

                    if (lstCategory.Count == 1)
                    {
                        HL_1.Visible = true;
                        HL_1.Text = get_categoryName(lstCategory[0]);
                        HL_1.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[0];
                    }
                    else if (lstCategory.Count == 2)
                    {
                        HL_1.Visible = true;
                        HL_1.Text = get_categoryName(lstCategory[0]);
                        HL_1.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[0];

                        HL_2.Visible = true;
                        HL_2.Text = get_categoryName(lstCategory[1]);
                        HL_2.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[1];
                    }
                    else if (lstCategory.Count > 2)
                    {
                        HL_1.Visible = true;
                        HL_1.Text = get_categoryName(lstCategory[lstCategory.Count - 2]);
                        HL_1.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[lstCategory.Count - 2];

                        HL_2.Visible = true;
                        HL_2.Text = get_categoryName(lstCategory[lstCategory.Count - 1]);
                        HL_2.NavigateUrl = "/reservationarea/extraservicecategory.aspx?id=" + lstCategory[lstCategory.Count - 1];
                    }
                }
                lbl_current.Text = "ExtraService Booking";
            }

            return true;
        }
        protected string get_categoryName(int catid)
        {
            using (DCmodRental dc = new DCmodRental())
            {
                dbRntExtrasCategoryLN currTBLCategory = dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == catid && x.pidLang == langId);
                if (currTBLCategory == null || string.IsNullOrEmpty(currTBLCategory.title))
                    currTBLCategory = dc.dbRntExtrasCategoryLNs.SingleOrDefault(x => x.pidCategory == catid && x.pidLang == 2);
                if (currTBLCategory != null)
                {
                    return currTBLCategory.title;
                }
                else
                {
                    return "";
                }
            }
        }

    }
}