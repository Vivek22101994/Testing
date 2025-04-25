<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stp_privacy.aspx.cs" Inherits="RentalInRome.stp_privacy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
    <link href="<%=CurrentAppSettings.ROOT_PATH %>css/style.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		body {background-color:#FFF;
		      background-image: none;
		      padding: 15px;font-family: "Trebuchet MS",Arial,Helvetica,sans-serif;color: #999999;font-size: 11px;}
	</style>
</head>
<body>
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <form id="form1" runat="server">
	<div>
	<%= ltr_description.Text %>
	</div>
    </form>
</body>
</html>
