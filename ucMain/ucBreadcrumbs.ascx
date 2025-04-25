<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBreadcrumbs.ascx.cs" Inherits="RentalInRome.ucMain.ucBreadcrumbs" %>

<div class="nulla">
</div>
<ul id="breadcrumbs" class="breadcrumb">
    <li><a href="<%=CurrentSource.getPagePath("1", "stp", CurrentLang.ID.ToString()) %>" class="breadLink">Home</a> </li>
    <li runat="server" id="li_1" visible="false">
        <asp:HyperLink ID="HL_1" runat="server" CssClass="breadLink">HyperLink</asp:HyperLink>
    </li>
    <li runat="server" id="li_2" visible="false">
        <asp:HyperLink ID="HL_2" runat="server" CssClass="breadLink">HyperLink</asp:HyperLink>
    </li>
    <li runat="server" id="li_3" visible="false">
        <asp:HyperLink ID="HL_3" runat="server" CssClass="breadLink">HyperLink</asp:HyperLink>
    </li>
    <li>
        <asp:Label ID="lbl_current" runat="server" CssClass="currentPage" Text="Current Page"></asp:Label>
    </li>
</ul>
<div class="nulla">
</div>
