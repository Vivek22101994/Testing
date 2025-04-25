using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RentalInRome.data;
using ModInvoice;
using System.Net;

namespace RentalInRome
{
    public partial class util_bancasella3d_redir : mainBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DCmodInvoice dc = new DCmodInvoice())
            {   
                var redirUrl = "https://ecomm.sella.it/pagam/pagam3d.aspx ";
                var mainUrl = global::RentalInRome.Properties.Settings.Default.RentalInRome_WsBancaSellaS2S_WSs2s;
                if (mainUrl == null || mainUrl.Contains("testecomm.sella.it"))
                    redirUrl = "https://testecomm.sella.it/pagam/pagam3d.aspx";

                string uid = Request.QueryString["uid"];
                var currTxn = dc.dbInvPayBancaSellaVerifiedByVisaTXNs.SingleOrDefault(x => x.uid.ToString() == uid);
                if (currTxn == null) 
                {
                    Response.Clear();
                    Response.Write("ERROR!");
                    Response.End();
                    return;
                }
                var _currTBL = maga_DataContext.DC_RENTAL.RNT_TBL_RESERVATION.SingleOrDefault(x => x.id == currTxn.reservationId);
                if (_currTBL == null)
                {
                    Response.Clear();
                    Response.Write("ERROR!");
                    Response.End();
                    return;
                }
                RemotePost _post = new RemotePost();
                _post.Target = "_top";
                _post.Add("a", CommonUtilities.getSYS_SETTING("utilsBancaSellaCodiceEsercente"));
                _post.Add("b", currTxn.VbVRisp);
                _post.Add("c", App.HOST_SSL + "/util_bancasella3d_ipn.aspx?uid=" + uid);
                _post.Url = redirUrl;
                _post.AutoPost = Request.QueryString["AutoPost"] == "true";
                if (_post.AutoPost)
                {
                    _post.HeadContent = ltrHead.Text.Replace("#lblVerifiedByVisaClick#", contUtils.getLabel("lblVerifiedByVisaClick", _currTBL.cl_pid_lang.objToInt32(), "Click here"));
                    _post.BodyContent = ltrBody.Text.Replace("#lblVerifiedByVisaClick#", contUtils.getLabel("lblVerifiedByVisaClick", _currTBL.cl_pid_lang.objToInt32(), "Click here"));
                }
                else
                {
                    _post.HeadContent = ltrHeadNoLoading.Text.Replace("#lblVerifiedByVisaClick#", contUtils.getLabel("lblVerifiedByVisaClick", _currTBL.cl_pid_lang.objToInt32(), "Click here"));
                    _post.BodyContent = ltrBodyNoLoading.Text.Replace("#lblVerifiedByVisaClick#", contUtils.getLabel("lblVerifiedByVisaClick", _currTBL.cl_pid_lang.objToInt32(), "Click here"));
                }

                //set new TLS protocol 1.1/1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;  

                _post.Post();

            }
        }
    }
}
