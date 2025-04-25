<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_menu_breadcrumb.ascx.cs" Inherits="RentalInRome.uc.UC_menu_breadcrumb" %>
<div class="nulla">
</div>
<ul id="breadcrumbs" class="breadcrumb">
   <li> <a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>" class="breadLink">Home</a> </li>
   <li> <asp:HyperLink ID="HL_1" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink> </li>
   <li> <asp:HyperLink ID="HL_2" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink> </li>
   <li> <asp:HyperLink ID="HL_3" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink> </li>
   <li> <asp:Label ID="lbl_current" runat="server" CssClass="currentPage" Text="Current Page"></asp:Label> </li>
</ul>
<div class="nulla">
</div>
