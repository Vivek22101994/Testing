<%@ Page Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="pg_business_housing.aspx.cs" Inherits="RentalInRome.pg_business_housing" %>


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
	<asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal>
	
	<h3 class="underlined" style="margin-left: 14px;"><%= ltr_title.Text%></h3>
	
	<div class="nulla"></div>
	
	<div id="lista_search" class="zona" >
		<div class="sx">
			<%= ltr_description.Text%>
		</div>
		<div class="dx">
			
			<div class="ordina">
				<span class="ord_sx">Ordina</span>
				<div class="ord_dx">
					<a href="#">
						<span>Prezzo</span></a>
					<a href="#">
						<span>Distanza</span></a>
				</div>
			</div>
			<div class="ico">
				<a href="#" class="switch_thumb">Switch Thumb</a>
				<a class="ico_map" href="#"></a>
			</div>
		</div>
	</div>
	
	
</asp:Content>
