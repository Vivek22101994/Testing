<%@ Page Language="C#" MasterPageFile="~/common/MP_main.Master" AutoEventWireup="true" CodeBehind="pg_BookNow_details.aspx.cs" Inherits="RentalInRome.details_BookNow" %>


<%@ Register Src="uc/UC_header.ascx" TagName="UC_header" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_block.ascx" TagName="UC_static_block" TagPrefix="uc1" %>
<%@ Register Src="uc/UC_static_collection.ascx" TagName="UC_static_collection" TagPrefix="uc2" %>
<%@ Register Src="uc/UC_apt_in_rome_bottom.ascx" TagName="UC_apt_in_rome_bottom" TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="CPH_head_top" runat="server">
    <title>
        <%=ltr_meta_title.Text%></title>
    <meta name="description" content="<%=ltr_meta_description.Text %>" />
    <meta name="keywords" content="<%=ltr_meta_keywords.Text %>" />
</asp:content>
<asp:content id="Content2" contentplaceholderid="CPH_head_bottom" runat="server">
</asp:content>
<asp:content id="Content3" contentplaceholderid="CPH_main" runat="server">
    <asp:Literal runat="server" ID="ltr_meta_title" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_description" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_meta_keywords" Visible="false"></asp:Literal>
    <asp:Literal runat="server" ID="ltr_img_banner" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_sub_title" Visible="false"></asp:Literal>
	<asp:Literal runat="server" ID="ltr_description" Visible="false"></asp:Literal> 
	
	
	<h3 class="underlined">Book Now</h3>
	
	<div class="nulla">
	</div>
	 
	<div id="contatti">
		<div class="sx">
			
			<div class="riep_sx">
				<h1>Apartment Trastevere Luxury</h1>
				<span>city: <strong>Roma</strong></span>
				<span>zone: <strong>Trastevere</strong></span>
				<br />
				<span>check-in: <strong>14/05/2011</strong></span>
				<span>check-out: <strong>20/05/2011</strong></span>
				<span>Notti: <strong>6</strong></span>
				<br />
				<span>Aduti: <strong>2</strong> &nbsp;&nbsp; Bambini: <strong>1</strong></span>
			
				<img src="http://www.rentalinrome.com/crispimarbleluxury/foto17.jpg"  />
				<div class="nulla">
				</div>
			</div>
		</div>

		<div class="dx" >
		</div>
		
		<div class="nulla"></div>
	</div>

	<uc3:UC_apt_in_rome_bottom ID="UC_apt_in_rome_bottom1" runat="server" />

</asp:content>
