using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFileManager.wfm
{
    public partial class upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HF_return.Value = Request.QueryString["return"];
                HF_callback.Value = Request.QueryString["callback"];
                HF_referer.Value = Request.QueryString["referer"];
                HF_folder.Value = Request.QueryString["folder"];
            }
        }

        protected void btn_uplaod_Click(object sender, EventArgs e)
        {
            if(!FU_uplaod.HasFile) return;
            string _name = "";
            string _folder = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_folder.Value);
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
            _name = FU_uplaod.FileName;
            if (File.Exists(Path.Combine(_folder, _name)))
            {
                int _extIndex = _name.LastIndexOf(".");
                string _mName = _name.Substring(0, _extIndex);
                string _extName = _name.Substring(_extIndex + 1).ToLower();
                int i = 1;
                while (File.Exists(Path.Combine(_folder, _mName+"-"+i+"."+_extName)))
                {
                    i++;
                }
                _name = _mName + "-" + i + "." + _extName;
            }
            string FullName = Path.Combine(_folder, _name);
            FU_uplaod.SaveAs(FullName);
            Response.Redirect(HF_referer.Value + "?return=" + HF_return.Value + "&callback=" + HF_callback.Value + "&folder=" + HF_folder.Value);
        }
        protected void ibtn_back_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(HF_referer.Value + "?return=" + HF_return.Value + "&callback=" + HF_callback.Value + "&folder=" + HF_folder.Value);
        }
    }
}
