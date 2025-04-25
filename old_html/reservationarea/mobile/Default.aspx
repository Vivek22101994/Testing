<%@ Page Title="" Language="C#" MasterPageFile="masterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RentalInRome.reservationarea.mobile.Default" %>

<%@ Register Src="ucHeader.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
    <title>Reservation Area</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <asp:HiddenField ID="HF_unique" runat="server" Value="" Visible="false" />
    <uc1:ucHeader ID="ucHeader" runat="server" MenuType="home" />
</asp:Content>
