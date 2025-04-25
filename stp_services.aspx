<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/common/MP_main.Master" CodeBehind="stp_services.aspx.cs" Inherits="RentalInRome.stp_services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
	<title>
		<%=ltr_meta_title.Text%></title>
	<meta name="description" content="<%=ltr_meta_description.Text %>" />
	<meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
	<asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
	
	<div id="testoMain" style="background: none; margin: 0 25px; width: 852px;">
		<h3 class="underlined" style="margin-left: 0;"><%= ltr_title.Text%></h3>
		<div class="nulla"></div>
		<div class="main_services">
			<%= ltr_description.Text%>
		</div>
	</div>
	
</asp:Content>
