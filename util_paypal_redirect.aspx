<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="util_paypal_redirect.aspx.cs" Inherits="RentalInRome.util_paypal_redirect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<asp:Literal ID="ltrHead" runat="server">
    <title>We are redirecting you to Secure Payment Page by PayPal</title>
    <style type="text/css">
        #loadingPayPal {position: absolute;width: 100%;height: 150px;margin-top:200px;background-color: #fe6634;}
        #loadingPayPal > div {width:960px;margin:0 auto; height:150px;}
        #loadingPayPal img {float:left;}
        #loadingPayPal span {display:block;float:left;font-weight:bold;font-style:italic;font-family:Arial, Helvetica, sans-serif;width:520px;}
        input.submit {background: none repeat scroll 0% 0% transparent; border: medium none; padding: 0px; margin: 20px 0px 0px; color: #585859; cursor: pointer;}
    </style>
</asp:Literal>
<asp:Literal ID="ltrBody" runat="server">
    <div id="loadingPayPal">
        <div>
            <img src="/images/loading-paypal.gif" width="435" height="150" alt="loading...We are redirecting you to Secure Payment by PayPal page" align="left" style="" />
            <span style="color: #FFF; font-size: 30px; line-height: 30px; margin-top: 30px;">LOADING...</span>
            <span style="color: #4b4c4f; font-size: 18px; line-height: 20px;">We are redirecting you to Secure Payment Page by PayPal</span>
            <input type="submit" class="submit" value="If you are waiting for more than 5 seconds please click here" />
        </div>
    </div>
</asp:Literal>

<asp:Literal ID="ltrMobileHead" runat="server">
    <title>We are redirecting you to Secure Payment Page by PayPal</title>
    <style type="text/css">
        
        * {
	        margin:0;
	        padding:0;
	        font-family: Arial, Verdana, Helvetica, sans-serif;
        }
        #loadingPayPal {position: absolute;width: 100%;}
        #loadingPayPal > div {margin:0 auto;}
        #loadingPayPal img {float:left;width: 100%;}
        #loadingPayPal span {display:block;float:left;font-weight:bold;font-style:italic; margin: auto 20px;}
        input.submit {background: none repeat scroll 0% 0% transparent; border: medium none; padding: 0px; margin: 20px 0px 0px; color: #585859; cursor: pointer;}
    </style>
</asp:Literal>
<asp:Literal ID="ltrMobileBody" runat="server">
    <div id="loadingPayPal">
        <div>
            <span style="color: #fe6634; font-size: 30px; line-height: 30px; margin-top: 30px;">LOADING...</span>
            <span style="color: #4b4c4f; font-size: 18px; line-height: 20px;">We are redirecting you to Secure Payment Page by PayPal</span>
            <input type="submit" class="submit" value="Waiting for more than 5 seconds? Please click here." />
            <img src="/images/loading-paypal.gif" alt="loading...We are redirecting you to Secure Payment by PayPal page" align="left" style="" />
        </div>
    </div>
</asp:Literal>

<asp:Literal ID="ltrHeadWL" runat="server">
    <title>We are redirecting you to Secure Payment Page by PayPal</title>   
    <style type="text/css">
        #loadingPayPal {position: absolute;width: 100%;height: 150px;margin-top:200px;background-color: #c4c4c4;}
        #loadingPayPal > div {width:960px;margin:0 auto; height:150px;}
        #loadingPayPal img {float:left;}
        #loadingPayPal span {display:block;float:left;font-weight:bold;font-style:italic;font-family:Arial, Helvetica, sans-serif;width:520px;}
        input.submit {background: none repeat scroll 0% 0% transparent; border: medium none; padding: 0px; margin: 20px 0px 0px; color: #585859; cursor: pointer;}
    </style>
</asp:Literal>
<asp:Literal ID="ltrBodyWL" runat="server">    
    <div id="loadingPayPal">
        <div>
            <img src="WLRental/images/loading-paypal-wl.gif" width="435" height="150" alt="loading...We are redirecting you to Secure Payment by PayPal page" align="left" style="">
            <span style="color: #FFF; font-size: 30px; line-height: 30px; margin-top: 30px;">LOADING...</span>
            <span style="color: #4b4c4f; font-size: 18px; line-height: 20px;">We are redirecting you to Secure Payment Page by PayPal</span>
            <input type="submit" class="submit" value="If you are waiting for more than 5 seconds please click here">
        </div>
    </div>
</asp:Literal>
    </form>
</body>
</html>
