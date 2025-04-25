using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModRental;

namespace RentalInRome.webservice
{
    public partial class contLabelXml : System.Web.UI.Page
    {
        private int _currLang;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                _currLang = Request["lang"].objToInt32() == 0 ? CurrentLang.ID : Request["lang"].objToInt32();
                List<string> lblList = ("" + Request["lbls"]).splitStringToList("|").Where(x => !string.IsNullOrEmpty(x)).ToList();
                Response.Write(getList(lblList));
                Response.End();
            }
        }
        private string getList(List<string> lblList)
        {
            string returnString = "";
            returnString += "<label_list lang=\"" + _currLang + "\">";
            if (lblList.Count == 0)
            {
                var list = contProps.LabelTBL.Where(x => x.pidLang == _currLang && !string.IsNullOrEmpty(x.title)).ToList();
                foreach (var tmp in list)
                {
                    returnString += "<label>";
                    returnString += "<label_name>" + tmp.id + "</label_name>";
                    returnString += "<label_value>" + tmp.title.htmlEncode() + "</label_value>";
                    returnString += "</label>";
                }
            }
            else
            {
                foreach (var tmp in lblList)
                {
                    returnString += "<label>";
                    returnString += "<label_name>" + tmp + "</label_name>";
                    returnString += "<label_value>" + contUtils.getLabel_title(tmp, _currLang, tmp).Trim().htmlEncode() + "</label_value>";
                    returnString += "</label>";
                }
            }
            returnString += "</label_list>";
            return returnString;
        }
    }
}
