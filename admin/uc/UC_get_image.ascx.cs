using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentalInRome.admin.uc
{
    public partial class UC_get_image : System.Web.UI.UserControl
    {
        public bool ShowImg
        {
            get { return HF_show_img.Value == "1"; }
            set { HF_show_img.Value = value ? "1" : "0"; }
        }
        public bool ShowPath
        {
            get { return HF_show_path.Value == "1"; }
            set { HF_show_path.Value = value ? "1" : "0"; }
        }
        public bool ShowCrop
        {
            get { return HF_show_crop.Value == "1"; }
            set { HF_show_crop.Value = value ? "1" : "0"; }
        }
        public string ImgPath
        {
            get { return HF_img.Value; }
            set { HF_img.Value = value; setImg(); }
        }
        public string ImgWidth
        {
            set { img.Style.Add("width", value); }
        }
        public string ImgHeight
        {
            set { img.Style.Add("height", value); }
        }
        public string ImgMaxWidth
        {
            set { img.Style.Add("max-width", value); }
        }
        public string ImgMaxHeight
        {
            set { img.Style.Add("max-height", value); }
        }
        public bool ImgCropAspectRatio
        {
            get { return HF_imgCropAspectRatio.Value == "true"; }
            set { HF_imgCropAspectRatio.Value = value ? "true" : "false"; }
        }
        public string ImgCropWidth
        {
            get { return HF_imgCropWidth.Value; }
            set { HF_imgCropWidth.Value = value; }
        }
        public string ImgCropHeight
        {
            get { return HF_imgCropHeight.Value; }
            set { HF_imgCropHeight.Value = value; }
        }
        public string ImgCropMaxWidth
        {
            get { return HF_imgCropMaxWidth.Value; }
            set { HF_imgCropMaxWidth.Value = value; }
        }
        public string ImgCropMaxHeight
        {
            get { return HF_imgCropMaxHeight.Value; }
            set { HF_imgCropMaxHeight.Value = value; }
        }
        public bool ImgCropIsVertical
        {
            get { return HF_imgCropIsVertical.Value == "true"; }
            set { HF_imgCropIsVertical.Value = value ? "true" : "false"; }
        }
        public string ImgCropContWidth
        {
            get { return HF_imgCropContWidth.Value; }
            set { HF_imgCropContWidth.Value = value; }
        }
        public string ImgCropContHeight
        {
            get { return HF_imgCropContHeight.Value; }
            set { HF_imgCropContHeight.Value = value; }
        }
        public string ImgRoot
        {
            get { return HF_root.Value; }
            set
            {
                HF_root.Value = value; HL_select.NavigateUrl = "javascript:OpenShadowbox('" + CurrentAppSettings.ROOT_PATH + "wfm/get_image.aspx?callback=parent." + Unique + "_setImage&return=PathRoot&folder=" + value + "', 500, 0)";
            }
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = Guid.NewGuid().ToString().Replace("-", "_").Replace("1", "q").Replace("2", "w").Replace("3", "e").Replace("4", "r").Replace("5", "t").Replace("6", "y").Replace("7", "u").Replace("8", "i").Replace("9", "o").Replace("0", "p");
                return HF_unique.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PH_show_img.Visible = ShowImg;
                PH_show_path.Visible = ShowPath;
                HL_select.NavigateUrl = "javascript:OpenShadowbox('" + CurrentAppSettings.ROOT_PATH + "wfm/get_image.aspx?callback=parent." + Unique + "_setImage&return=PathRoot&folder=" + ImgRoot + "', 500, 0)";
                HL_crop.Visible = ShowCrop;
                string _script = "var " + Unique + "_contentUpdater = '" + btn_page_update.ClientID + "';";
                _script += "function  " + Unique + "_ReloadContent() {";
                _script += "buttonPostBack( " + Unique + "_contentUpdater);";
                _script += "}";
                _script += "function  " + Unique + "_setImage(img) {";
                _script += "$get('" + HF_img.ClientID + "').value = '' + img;";
                _script += "Shadowbox.close();";
                _script += Unique + "_ReloadContent();";
                _script += "}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Unique, _script, true);
            }
        }

        protected void setImg()
        {
            img.ImageUrl = CurrentAppSettings.ROOT_PATH + HF_img.Value;
            txt_img.Text = HF_img.Value;
            string url = "" + CurrentAppSettings.ROOT_PATH + "wfm/crop_image.aspx?callback=parent." + Unique + "_setImage&return=PathRoot";
            url += "&file_name=" + HF_img.Value;
            url += "&sel_ar=" + HF_imgCropAspectRatio.Value;
            url += "&sel_w=" + ImgCropWidth;
            url += "&sel_h=" + ImgCropHeight;
            url += "&sel_mw=" + ImgCropMaxWidth;
            url += "&sel_mh=" + ImgCropMaxHeight;
            url += "&imgContHeight=" + ImgCropContHeight;
            url += "&imgContWidth=" + ImgCropContWidth;
            url += "&isVertical=" + ImgCropIsVertical;
            url += "";
            HL_crop.NavigateUrl = "javascript:OpenShadowbox('" + url + "', 880, 0)";
        }

        protected void btn_page_update_Click(object sender, EventArgs e)
        {
            setImg();
        }
    }
}