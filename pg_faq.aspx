<%@ Page Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="pg_faq.aspx.cs" Inherits="RentalInRome.pg_faq" %>


<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_Rental_Recommended.ascx" TagName="UC_Rental_Recommended" TagPrefix="uc3" %>
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
	
<div id="testoMain">
	<h3 class="underlined">
		<%= ltr_title.Text%></h3>
	<div class="mainContent">
		<%= ltr_description.Text%>
	</div>
</div>
<div id="colDx">
	<h3 class="underlined">
		<%= CurrentSource.getSysLangValue("lblOurWebsiteIsRecommendedBy")%>
	</h3>
	<uc3:UC_Rental_Recommended ID="UC_Rental_Recommended1" runat="server" />
</div>
<div class="nulla">
</div>

<uc1:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />

</asp:Content>