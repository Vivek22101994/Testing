<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_menu_breadcrumb.ascx.cs" Inherits="RentalInRome.WLRental.uc.UC_menu_breadcrumb" %>
<div id="breadcrumbs">
    <a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>" class="breadLink">Home</a>
    <asp:HyperLink ID="HL_1" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink>
    <asp:HyperLink ID="HL_2" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink>
    <asp:HyperLink ID="HL_3" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink>
    <asp:Label ID="lbl_current" runat="server" CssClass="currentPage" Text="Current Page"></asp:Label>
</div>
