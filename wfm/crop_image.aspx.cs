using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;

namespace WebFileManager.wfm
{
    public partial class crop_image : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HF_return.Value = Request.QueryString["return"];
                HF_callback.Value = Request.QueryString["callback"];
                HF_referer.Value = Request.QueryString["referer"];
                HF_folder.Value = Request.QueryString["folder"];
                HF_file_name.Value = Request.QueryString["file_name"];
                HF_file_name_new.Value = "/" + Request.QueryString["file_name"];
                HF_sel_w.Value = String.IsNullOrEmpty(Request.QueryString["sel_w"]) ? "50" : Request.QueryString["sel_w"];
                HF_sel_h.Value = String.IsNullOrEmpty(Request.QueryString["sel_h"]) ? "50" : Request.QueryString["sel_h"];
                HF_sel_mw.Value = String.IsNullOrEmpty(Request.QueryString["sel_mw"]) ? "null" : Request.QueryString["sel_mw"];
                HF_sel_mh.Value = String.IsNullOrEmpty(Request.QueryString["sel_mh"]) ? "null" : Request.QueryString["sel_mh"];
                HF_sel_ar.Value = Request.QueryString["sel_ar"] == "true" ? "true" : "false";
                HF_imgContHeight.Value = String.IsNullOrEmpty(Request.QueryString["imgContHeight"]) ? "300" : Request.QueryString["imgContHeight"];
                HF_imgContWidth.Value = String.IsNullOrEmpty(Request.QueryString["imgContWidth"]) ? "400" : Request.QueryString["imgContWidth"];
                HF_isVertical.Value = !String.IsNullOrEmpty(Request.QueryString["isVertical"]) && Request.QueryString["isVertical"].Trim().ToLower() == "true" ? "true" : "false";
                Bitmap img = (Bitmap)Bitmap.FromFile(Server.MapPath("/" + HF_file_name.Value));
                //Original Values
                int _width = img.Width;
                int _height = img.Height;
                HF_imgWidth.Value = _width.ToString();
                HF_imgHeight.Value = _height.ToString();
            }
        }

        protected void lnk_save_Click(object sender, EventArgs e)
        {
            if (HF_referer.Value!="")
                Response.Redirect(HF_referer.Value + "?return=" + HF_return.Value + "&callback=" + HF_callback.Value + "&folder=" + HF_folder.Value + "&file_name=" + HF_file_name.Value);
            else if (HF_file_name_new.Value != "")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "end_crop", HF_callback.Value + "('" + HF_file_name_new.Value.Substring(1, HF_file_name_new.Value.Length - 1) + "');", true);
        }
    }
}
