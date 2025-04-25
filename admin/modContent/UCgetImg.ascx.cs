using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Editor.DialogControls;

namespace ModContent.admin.modContent
{
    public partial class UCgetImg : System.Web.UI.UserControl
    {
        public bool ShowImg
        {
            get { return HF_show_img.Value == "1"; }
            set { HF_show_img.Value = value ? "1" : "0"; PH_show_img.Visible = ShowImg; }
        }
        public bool ShowPath
        {
            get { return HF_show_path.Value == "1"; }
            set { HF_show_path.Value = value ? "1" : "0"; PH_show_path.Visible = ShowPath; }
        }
        public string ImgPath
        {
            get { return HF_imgPath.Value; }
            set
            {
                if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(ImgPathDef))
                    HF_imgPath.Value = ImgPathDef;
                else
                    HF_imgPath.Value = value;
                txt_img.Text = HF_imgPath.Value;
                img.ImageUrl = App.RP + HF_imgPath.Value + "?" + DateTime.Now.JSCal_dateTimeToString();
            }
        }
        public string ImgPathDef
        {
            get { return HF_imgPathDef.Value; }
            set { HF_imgPathDef.Value = value; }
        }
        public string ImgName
        {
            get { return HF_imgName.Value; }
            set { HF_imgName.Value = value; }
        }
        public string ImgExtension
        {
            get { return HF_imgExtension.Value; }
            set { HF_imgExtension.Value = value; }
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
        public string ImgRoot
        {
            get { return HF_root.Value; }
            set
            {
                HF_root.Value = value;
            }
        }
        public string Unique
        {
            get
            {
                if (HF_unique.Value == "")
                    HF_unique.Value = string.Empty.createUniqueID();
                return HF_unique.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AsyncUpload1.OnClientFileUploaded = "fileUploaded_" + Unique;
            AsyncUpload1.OnClientValidationFailed = "validationFailed_" + Unique;
            if (!IsPostBack)
            {

                PH_show_img.Visible = ShowImg;
                PH_show_path.Visible = ShowPath;
            }
            if (!IsPostBack)
            {
                string _script = "";
                _script = "function  fileUploaded_" + Unique + "() {";
                _script += "    $find('" + rapMain.ClientID + "').ajaxRequest('');$telerik.$(\".invalid\").html(\"\");setTimeout(function () {sender.deleteFileInputAt(0);}, 10);";
                _script += "}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "fileUploaded_" + Unique, _script, true);

                _script = "function  validationFailed_" + Unique + "(sender, args) {";
                _script += "    $telerik.$(\".invalid\").html(\"Estensione Non valida, seleziona un file con est. jpeg,jpg,gif,png,bmp,pdf\");sender.deleteFileInputAt(0);";
                _script += "}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validationFailed_" + Unique, _script, true);
                _script = "function  DialogCallBack_" + Unique + "(sender, args) {";
                _script += "    if (!args) {return false;} $find('" + rapMain.ClientID + "').ajaxRequest(''+args.value[0].getAttribute('src', 2));";
                _script += "}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DialogCallBack_" + Unique, _script, true);

                _script = "function  DialogOpen_" + Unique + "(sender, args) {";
                _script += "    $find('" + DialogOpener1.ClientID + "').open('ImageManager', {CssClasses: []}); args.set_cancel(true);";
                _script += "}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DialogOpen_" + Unique, _script, true);
                btnDialogOpen.OnClientClicking = "DialogOpen_" + Unique;
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            setDialogManagers();
        }
        protected void setDialogManagers()
        {
            FileManagerDialogParameters imageManagerParameters = new FileManagerDialogParameters();
            imageManagerParameters.ViewPaths = new string[] { "~/" + ImgRoot };
            imageManagerParameters.UploadPaths = new string[] { "~/" + ImgRoot };
            imageManagerParameters.DeletePaths = new string[] { "~/" + ImgRoot };
            imageManagerParameters.MaxUploadFileSize = 5000000;
            imageManagerParameters.SearchPatterns = new string[] { "*.jpg", "*.gif", "*.png", "*.jpeg", "*.tiff", "*.tif", "*.jpe", "*.rgb", "*.bmp" };//, *.xbm, *.xpm, , *.ief, , , , , , , *.g3f, *.xwd, *.pict, *.ppm, *.pgm, *.pbm, *.pnm, , *.ras, *.pcd, *.cgm, *.mil, *.cal, *.fif, *.dsf, *.cmx, *.wi, *.dwg, *.dxf, *.svf
            DialogDefinition imageManager = new DialogDefinition(typeof(ImageManagerDialog), imageManagerParameters);
            imageManager.ClientCallbackFunction = "DialogCallBack_" + Unique;
            imageManager.Width = Unit.Pixel(694);
            imageManager.Height = Unit.Pixel(440);
            imageManager.Modal = true;
            //If you need to customize the dialog then register the external dialog files  
            //imageManager.Parameters["ExternalDialogsPath"] = "~/EditorDialogs/";  
            DialogOpener1.DialogDefinitions.Remove("ImageManager");
            DialogOpener1.DialogDefinitions.Add("ImageManager", imageManager);

            FileManagerDialogParameters imageEditorParameters = new FileManagerDialogParameters();
            imageEditorParameters.ViewPaths = new string[] { "~/" + ImgRoot };
            imageEditorParameters.UploadPaths = new string[] { "~/" + ImgRoot };
            imageEditorParameters.DeletePaths = new string[] { "~/" + ImgRoot };
            imageEditorParameters.MaxUploadFileSize = 5000000;

            DialogDefinition imageEditor = new DialogDefinition(typeof(ImageEditorDialog), imageEditorParameters);
            imageEditor.Width = Unit.Pixel(832);
            imageEditor.Height = Unit.Pixel(520);
            imageEditor.Modal = true;
            DialogOpener1.DialogDefinitions.Remove("ImageEditor");
            DialogOpener1.DialogDefinitions.Add("ImageEditor", imageEditor);
        }
        protected void rapMain_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Argument))
            {
                ImgPath = e.Argument.Substring(1, e.Argument.Length - 1);
            }
        }
        protected void btn_clearImg_Click(object sender, EventArgs e)
        {
            ImgPath = "";
        }
        protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            if (ImgName == "")
            {
                ImgName = e.File.GetNameWithoutExtension();
            }
            ImgExtension = e.File.GetExtension();
            ImgPath = ImgRoot + "/" + ImgName + ImgExtension;
            if (File.Exists(Path.Combine(App.SRP, ImgPath)))
            {
                int index = 0;
                while (File.Exists(Path.Combine(App.SRP, ImgPath)))
                {
                    index++;
                    ImgPath = ImgRoot + "/" + ImgName + "_" + index + ImgExtension;
                }
            }
            e.File.SaveAs(Path.Combine(App.SRP, ImgPath), true);
            img.ImageUrl = App.RP + ImgPath + "?" + DateTime.Now.JSCal_dateTimeToString();
        }
    }
}