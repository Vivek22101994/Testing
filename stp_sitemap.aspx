<%@ Page Title="" Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="stp_sitemap.aspx.cs" Inherits="RentalInRome.stp_sitemap" %>

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
	<div id="contenuto_neg">
		<div class="mappa">
			<h1 class="cu">Site-map</h1>
			<div class="primi">
				<a href="#"><strong>Home</strong></a>
			</div>
			<div class="primi">
				<a href="#"><strong>Negozio</strong></a>
			</div>
			<div class="primi">
				<a href="#"><strong>Gioielli</strong></a>
				<div class="secondo">
					<a class="sec" href="#">Anelli</a>
					<div class="terzo">
						<a href="#">Collezione Anelli</a>
						<div class="quarto">
							<a href="#">Scheda tecnica</a>
						</div>
					</div>
				</div>
				<div class="secondo">
					<a class="sec" href="#">Collane</a>
					<div class="terzo">
						<a href="#">Collezione Collane</a>
						<div class="quarto">
							<a href="#">Scheda tecnica</a>
						</div>
					</div>
				</div>
				<div class="secondo">
					<a class="sec" href="#">Bracciali</a>
					<div class="terzo">
						<a href="#">Collezione Bracciali</a>
						<div class="quarto">
							<a href="#">Scheda tecnica</a>
						</div>
					</div>
				</div>
				<div class="secondo">
					<a class="sec" href="#">Orecchini</a>
					<div class="terzo">
						<a href="#">Collezione Orecchini</a>
						<div class="quarto">
							<a href="#">Scheda tecnica</a>
						</div>
					</div>
				</div>
			</div>
			<div class="primi">
				<a href="#"><strong>Orologi</strong></a>
			</div>
			<div class="primi">
				<a href="#"><strong>Servizi</strong></a>
			</div>
			<div class="primi">
				<a href="#"><strong>Per lei</strong></a>
			</div>
			<div class="primi">
				<a href="#">P<strong>er lui</strong></a>
			</div>
		</div>
		<div class="nulla">
		</div>
	</div>
</asp:Content>
