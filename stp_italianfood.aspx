<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/common/MP_main.Master"  CodeBehind="stp_italianfood.aspx.cs" Inherits="RentalInRome.stp_italianfood" %>


<%@ Register Src="uc/UC_header.ascx" TagName="UC_header" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
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
	
	<%= ltr_title.Text != "" ? "<h3 class='underlined'>" + ltr_title.Text + " - " + ltr_sub_title.Text + "</h3>" : ""%>
	
	<div class="nulla">
	</div>
	<div id="imgWedding">
		<img src="<%=CurrentAppSettings.ROOT_PATH %>images/specialitalianfood/italian_food1.jpg" width="360px" alt="special italian food" />
		<img src="<%=CurrentAppSettings.ROOT_PATH %>images/specialitalianfood/italian_food4.jpg" width="360px" alt="special italian food" />
		<img src="<%=CurrentAppSettings.ROOT_PATH %>images/specialitalianfood/italian_food2.jpg" width="360px" alt="special italian food" />
	</div>
	<div class="txtWedding">
		<%=ltr_description.Text %>
	</div>
	<div class="nulla">
	</div>
</asp:Content>
