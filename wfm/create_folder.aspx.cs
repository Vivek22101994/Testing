using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFileManager.wfm
{
    public partial class create_folder : System.Web.UI.Page
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

        protected void btn_create_Click(object sender, EventArgs e)
        {
            string _name = "";
            string _folder = Path.Combine(CurrentAppSettings.SERVER_ROOT_PATH, HF_folder.Value);
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
            _name = ClearPathName(txt_folder_name.Text);
            if (!Directory.Exists(Path.Combine(_folder,_name)))
            {
                Directory.CreateDirectory(Path.Combine(_folder, _name));
            }
            Response.Redirect(HF_referer.Value + "?return=" + HF_return.Value + "&callback=" + HF_callback.Value + "&folder=" + HF_folder.Value + "/" + _name);
        }
        protected void ibtn_back_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(HF_referer.Value + "?return=" + HF_return.Value + "&callback=" + HF_callback.Value + "&folder=" + HF_folder.Value);
        }
        public static string ClearPathName(string path)
        {
            string returnValue = "" + path;
            returnValue = returnValue.Trim();
            returnValue = returnValue.ToLower();
            returnValue = returnValue.Replace("<br>", "-");
            returnValue = returnValue.Replace("<br/>", "-");
            returnValue = returnValue.Replace("<br />", "-");
            returnValue = returnValue.Replace("<p>", "-");
            returnValue = returnValue.Replace("</p>", "-");
            returnValue = returnValue.Replace(' ', '_');
            returnValue = returnValue.Replace('…', '_');
            returnValue = returnValue.Replace('è', 'e');
            returnValue = returnValue.Replace('é', 'e');
            returnValue = returnValue.Replace('ò', 'o');
            returnValue = returnValue.Replace('à', 'a');
            returnValue = returnValue.Replace('ù', 'u');
            returnValue = returnValue.Replace("'", "");
            returnValue = returnValue.Replace("\"", "-");
            returnValue = returnValue.Replace("’", "");
            returnValue = returnValue.Replace(",", "");
            returnValue = returnValue.Replace(".", "");
            returnValue = returnValue.Replace("?", "");
            returnValue = returnValue.Replace("!", "");
            returnValue = returnValue.Replace(":", "");
            returnValue = returnValue.Replace(";", "");
            returnValue = returnValue.Replace("/", "-");
            returnValue = returnValue.Replace("\\", "");
            returnValue = returnValue.Replace("(", "");
            returnValue = returnValue.Replace(")", "");
            returnValue = returnValue.Replace("&", "and");
            returnValue = returnValue.Replace("@", "a");
            returnValue = returnValue.Replace("<", "-");
            returnValue = returnValue.Replace(">", "-");
            if (returnValue.IndexOf("--") > -1)
                while (returnValue.IndexOf("--") > -1)
                    returnValue = returnValue.Replace("--", "-");

            return returnValue;
        }
    }
}
