<%@ Page Title="" Language="C#" MasterPageFile="~/reservationarea/mobile/masterPage.Master" AutoEventWireup="true" CodeBehind="arrivaldeparture.aspx.cs" Inherits="RentalInRome.reservationarea.mobile.arrivaldeparture" %>

<%@ Register Src="ucHeader.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_head_top" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_head_bottom" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_main" runat="server">
    <uc1:ucHeader ID="ucHeader" runat="server" />
    <div data-role="content" class="">
    </div>
</asp:Content>
