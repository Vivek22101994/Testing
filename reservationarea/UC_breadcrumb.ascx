<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_breadcrumb.ascx.cs" Inherits="RentalInRome.reservationarea.UC_breadcrumb" %>
<div id="breadcrumbs">
    <a href="/reservationarea/payment.aspx" class="breadLink"><%=CurrentSource.getSysLangValue("lblReservedArea")%></a>
    <asp:HyperLink ID="HL_1" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink>
    <asp:HyperLink ID="HL_2" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink>
    <asp:HyperLink ID="HL_3" runat="server" CssClass="breadLink" Visible="false">HyperLink</asp:HyperLink>
    <asp:Label ID="lbl_current" runat="server" CssClass="currentPage" Text="Current Page"></asp:Label>
    <asp:HiddenField ID="hfd_lang" runat="server"></asp:HiddenField>
      
</div>