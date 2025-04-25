using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFileManager.wfm
{
    public class WfmItem
    {
        private string _id;
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastAccess { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string PathFull { get; set; }
        public string PathRoot { get; set; }
        public string PathWeb { get; set; }
        public string Type { get; set; }
        public bool IsReadOnly { get; set; }
        public string Id { get { return _id; } set { _id = value; } }
        public float SizeByte { get; set; }
        public float SizeKiloByte { get; set; }
        public string SizeString { get; set; }
        // public string To { get; set; }
        public WfmItem()
        {
            this._id = Guid.NewGuid().ToString();
        }
    }
    public partial class get_image : System.Web.UI.Page
    {
        private string _root = "images";
        private string FFS0 = "0 bytes";
        private string FFSB = "{0:##.##} bytes";
        private string FFSGB = "{0:##.##} gb";
        private string FFSKB = "{0:##.##} kb";
        private string FFSMB = "{0:##.##} mb";
        protected string CurrentRoot
        {
            get { return ViewState["current_root"] as string; }
            set { ViewState["current_root"] = value; }
        }
        private List<WfmItem> CURRENT_WfmItems_;
        private List<WfmItem> CURRENT_WfmItems
        {
            get
            {
                if (CURRENT_WfmItems_ == null)
                    if (ViewState["CURRENT_WfmItems"] != null)
                    {
                        CURRENT_WfmItems_ =
                            PConv.DeserArrToList((object[])ViewState["CURRENT_WfmItems"],
                                                 typeof(WfmItem)).Cast<WfmItem>().ToList();
                    }
                    else
                        CURRENT_WfmItems_ = new List<WfmItem>();

                return CURRENT_WfmItems_;
            }
            set
            {
                ViewState["CURRENT_WfmItems"] = PConv.SerialList(value.Cast<object>().ToList());
                CURRENT_WfmItems_ = value;
            }
        }
        private string FormatFileSize(long Bytes)
        {
            Decimal size = 0;
            string result;

            if (Bytes >= 1073741824)
            {
                size = Decimal.Divide(Bytes, 1073741824);
                result =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        FFSGB,
                        size);
            }
            else if (Bytes >= 1048576)
            {
                size = Decimal.Divide(Bytes, 1048576);
                result =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        FFSMB,
                        size);
            }
            else if (Bytes >= 1024)
            {
                size = Decimal.Divide(Bytes, 1024);
                result =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        FFSKB,
                        size);
            }
            else if (Bytes > 0 & Bytes < 1024)
            {
                size = Bytes;
                result =
                    String.Format(
                        CultureInfo.InvariantCulture,
                        FFSB,
                        size);
            }
            else
            {
                result = FFS0;
            }

            return result;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HF_return.Value = Request.QueryString["return"];
                HF_callback.Value = Request.QueryString["callback"];
                if (Request.QueryString["folder"] != null && Request.QueryString["folder"] != "") _root = Request.QueryString["folder"];
                CurrentRoot = _root;
                Fill_LV(CurrentRoot);
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "Directory_In")
                {
                    Directory_In(HF_current_file.Value);
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showToolTip", "showToolTip();", true);
        }
        protected void Fill_LV(string Root)
        {
            ltr_folder.Text = "" + Root;
            string[] _files = Directory.GetFiles(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, Root));
            string[] _folders = Directory.GetDirectories(Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, Root));
            List<string> _data = new List<string>();
            _data.AddRange(_folders.ToList());
            _data.AddRange(_files.ToList());
            List<WfmItem> _list = new List<WfmItem>();
            foreach (string _f in _data)
            {
                FileInfo fi = new FileInfo(_f);
                WfmItem _wfm = new WfmItem();
                _wfm.DateCreated = fi.CreationTime;
                _wfm.DateLastAccess = fi.LastAccessTime;
                _wfm.DateModified = fi.LastWriteTime;
                _wfm.IsReadOnly = fi.IsReadOnly;
                _wfm.Name = fi.Name;
                _wfm.Extension = (string.IsNullOrEmpty(fi.Extension)) ? ((fi.Attributes == FileAttributes.Directory) ? "folder" : "unknown") : fi.Extension.Replace(".", string.Empty).ToLowerInvariant();
                _wfm.PathFull = fi.FullName;
                _wfm.PathRoot = "" + Root + "/" + _wfm.Name;
                _wfm.PathWeb = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + _wfm.PathRoot;
                _wfm.SizeByte = 0;
                _wfm.SizeKiloByte = 0;
                _wfm.SizeString = ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory) ? string.Empty : FormatFileSize(fi.Length);
                _wfm.Type = "";
                _list.Add(_wfm);
            }
            CURRENT_WfmItems = _list;
            LV.DataSource = CURRENT_WfmItems;
            LV.DataBind();
        }
        protected void Directory_In(string Directory)
        {
            CurrentRoot += "/" + Directory;
            Fill_LV(CurrentRoot);
        }
        protected void Directory_Out()
        {
            if (CurrentRoot.LastIndexOf('/')!=-1)
                CurrentRoot = CurrentRoot.Substring(0, CurrentRoot.LastIndexOf('/'));
            Fill_LV(CurrentRoot);
        }

        protected string getFileImg(string ext)
        {
            return "<img src='" + CurrentAppSettings.ROOT_PATH + "wfm/ico/" + ext + ".png'/>";
        }

        protected void LV_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl_name = e.Item.FindControl("lbl_name") as Label;
            //lbl_name.Attributes.Add("onclick", "return clickButton(event,'" + Button1.ClientID + "')");
        }

        protected void btn_page_update_Click(object sender, EventArgs e)
        {
            if (HF_action.Value == "Directory_In")
                Directory_In(HF_current_file.Value);
            if (HF_action.Value == "Directory_Out")
                Directory_Out();
            if (HF_action.Value == "get")
            {
                WfmItem _item = CURRENT_WfmItems.SingleOrDefault(x => x.Id == HF_current_id.Value);
                if(_item==null) return;
                string _return = "";
                if (HF_return.Value == "PathRoot")
                    _return = _item.PathRoot;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "end_get", "alert('" + _return + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "end_get", HF_callback.Value + "('" + _return + "');", true);
            }
        }

        protected void ibtn_upload_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("upload.aspx?return=" + HF_return.Value + "&callback=" + HF_callback.Value + "&referer=get_image.aspx&folder=" + CurrentRoot);
        }
        protected void ibtn_create_folder_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("create_folder.aspx?return=" + HF_return.Value + "&callback=" + HF_callback.Value + "&referer=get_image.aspx&folder=" + CurrentRoot);
        }
    }
}
