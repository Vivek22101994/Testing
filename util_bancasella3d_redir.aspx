<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="util_bancasella3d_redir.aspx.cs" Inherits="RentalInRome.util_bancasella3d_redir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<asp:Literal ID="ltrHead" runat="server">
    <title>We are redirecting you to Secure Authentication page</title>
    <style type="text/css">
        #loading {position: absolute;width: 100%;height: 150px;margin-top:200px;background-color: #fe6634;}
        #loading > div {width:960px;margin:0 auto; height:150px;}
        #loading img {float:left;}
        #loading span {display:block;float:left;font-weight:bold;font-style:italic;font-family:Arial, Helvetica, sans-serif;width:520px;}
        input.submit {background: none repeat scroll 0% 0% transparent; border: medium none; padding: 0px; margin: 20px 0px 0px; color: #585859; cursor: pointer;}
        
        </style>
</asp:Literal>
<asp:Literal ID="ltrHeadNoLoading" runat="server">
    <title></title>
    <style type="text/css">
        * {
            margin: 0;padding:0;
        }
        input.submit {background: none repeat scroll 0% 0% transparent; border: medium none; padding: 20px; margin: 0; color: #585859; cursor: pointer;}
        #clickpost { display:block; float:left; clear:both; width: 100%;}
        #clickpost input[class="submit"] { display:block; float:left; clear:both; font-weight:bold; background-color:#fe6634; color:#FFF; border: 1px solid #cb3f11; text-align:center; font-size:13px; line-height:13px; font-style:italic; padding:15px; border-radius:10px; box-shadow: 0 0 5px #C4C4DB; font-family:Arial;}
        #clickpost input[class="submit"]:hover { background:#cb3f11; border-color:#962b08;}
    </style>
</asp:Literal>
<asp:Literal ID="ltrBody" runat="server">
    <div id="loading">
        <div>
            <span style="color: #FFF; font-size: 30px; line-height: 30px; margin-top: 30px;">LOADING...</span>
            <span style="color: #4b4c4f; font-size: 18px; line-height: 20px;">We are redirecting you to Secure Authentication page</span>
            <input type="submit" class="submit" value="If you are waiting for more than 5 seconds please click here" />
        </div>
    </div>
</asp:Literal>
<asp:Literal ID="ltrBodyNoLoading" runat="server">
    <div id="clickpost">
        <div>
            <input type="submit" class="submit" value="#lblVerifiedByVisaClick#" />
        </div>
    </div>
</asp:Literal>
    </form>
</body>
</html>
