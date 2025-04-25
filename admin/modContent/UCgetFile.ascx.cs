using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace ModContent.admin.modContent
{
    public partial class UCgetFile : System.Web.UI.UserControl
    {
        public string FilePath
        {
            get { return HF_filePath.Value; }
            set
            {
                HF_filePath.Value = value;
                txt_file.Text = HF_filePath.Value;
                if (string.IsNullOrEmpty(value))
                {
                    HL.Visible = false;
                }
                else
                {
                    FileExtension = Path.GetExtension(value);
                    HL.NavigateUrl = App.RP + value + "?" + DateTime.Now.JSCal_dateTimeToString();
                    HL.CssClass = "icoFile " + FileExtension.Replace(".", "");
                    HL.Visible = true;
                }
            }
        }
        public string FileName
        {
            get { return HF_fileName.Value; }
            set { HF_fileName.Value = value; }
        }
        public string FileExtension
        {
            get { return HF_fileExtension.Value; }
            set { HF_fileExtension.Value = value; }
        }
        public string FileRoot
        {
            get { return HF_root.Value; }
            set { HF_root.Value = value; }
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
        public string AllowedFileExtensions
        {
            get { return AsyncUpload1.AllowedFileExtensions.ToList().listToString(","); }
            set { AsyncUpload1.AllowedFileExtensions = value.splitStringToList(",").ToArray(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AsyncUpload1.OnClientFileUploaded = "fileUploaded_" + Unique;
            AsyncUpload1.OnClientValidationFailed = "validationFailed_" + Unique;
            if (!IsPostBack)
            {
                string _script = "";
                _script += "function  fileUploaded_" + Unique + "() {";
                _script += "    $find('" + RadAjaxPanel1.ClientID + "').ajaxRequest();$telerik.$(\".invalid\").html(\"\");setTimeout(function () {sender.deleteFileInputAt(0);}, 10);";
                _script += "}";
                _script += "function  validationFailed_" + Unique + "(sender, args) {";
                _script += "    $telerik.$(\".invalid\").html(\"Estensione Non valida, seleziona un file con est. " + AllowedFileExtensions + "\");sender.deleteFileInputAt(0);";
                _script += "}";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Unique, _script, true);
            }
        }
        protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            if (FileName == "")
            {
                FileName = e.File.GetNameWithoutExtension();
            }
            FileExtension = e.File.GetExtension();
            FilePath = FileRoot + "/" + FileName + FileExtension;
            if (File.Exists(Path.Combine(App.SRP, FileName)))
            {
                int index = 0;
                while (File.Exists(Path.Combine(App.SRP, FileName)))
                {
                    index++;
                    FileName = FileRoot + "/" + FileName + "_" + index + FileExtension;
                }
            }
            e.File.SaveAs(Path.Combine(App.SRP, FilePath), true);
            HL.NavigateUrl = App.RP + FilePath + "?" + DateTime.Now.JSCal_dateTimeToString();
            HL.CssClass = "icoFile " + FileExtension.Replace(".", "");
            HL.Visible = true;
        }
    }
}