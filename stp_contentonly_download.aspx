<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stp_contentonly_download.aspx.cs" Inherits="RentalInRome.stp_contentonly_download" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
    <link href="<%=CurrentAppSettings.ROOT_PATH %>css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        @import url(/css/style.css);
        @import url(/css/common.css);
    </style>
</head>
<body style="background: none repeat scroll 0% 0% #FFF;">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
    <form id="form1" runat="server">
		<div id="main">
			<div>
                <div id="testoMain" style="width: auto;">
                    <%= ltr_title.Text != "" ? "<h3 class='underlined'>" + ltr_title.Text + "</h3>" : ""%>
                    <div class="mainContent">
                        <%=ltr_description.Text %>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
